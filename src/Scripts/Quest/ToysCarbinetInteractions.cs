using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Items;
using Assets.Scripts.Player;
using Assets.Scripts.Enums;
using MoreMountains.Feedbacks;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Assets.Scripts.Quest
{
    public class ToysCarbinetInteractions : Usable
    {
        public ObjectType ObjectType { get; private set; }
        private IEnumerable<Transform> Slots { get; set; }
        private PlayerStatus PlayerStatus => PlayerStatus.Instance;
        [SerializeField] private MMF_Player PlaceItemFeedback;
        [SerializeField] private MMF_Player CompleteQuestFeedback;
        private Dictionary<string, Transform> slotDictionary;

        private PlayerStatus playerStatus => PlayerStatus.Instance;

        #region Usable
        private void OnUse()
        {
            if (!PlayerStatus.EstaCarregandoItem) return;

            var espacoDisponivel = ObtemEspacoDisponivel();

            if (espacoDisponivel == null) return;

            ProgredirMissao();

            if (MissaoCompleta())
            {
                AlertaMissaoCompleta();
                CompleteQuestFeedback?.PlayFeedbacks();
                QuestLog.SetQuestState(QuestsEnum.GuardarBrinquedos, QuestState.ReturnToNPC);
            }
            else
            {
                PlayPlaceItemFeedback(espacoDisponivel);
            }
        }

        private void OnSelect()
        {
            return;
        }
        #endregion

        public new void Start()
        {
            InitializeObjectType();
            events.onUse.AddListener(OnUse);
            events.onSelect.AddListener(OnSelect);
            InitializeSlots();
            base.Start();
        }

        private void InitializeObjectType()
        {
            ObjectType = ObjectType.None;
        }

        private void InitializeSlots()
        {
            Slots = GetComponentsInChildren<Transform>().Where(t => t.CompareTag("Slot"));
            slotDictionary = new Dictionary<string, Transform>();
            foreach (var slot in Slots)
            {
                slotDictionary.Add(slot.name, slot);
            }
        }

        public Transform GetSlotById(string slotId)
        {
            slotDictionary.TryGetValue(slotId, out var slot);
            return slot;
        }

        private bool MissaoCompleta()
        {
            return DialogueLua.GetVariable("Quest1.TotalToysOnShelf").asInt >=
                DialogueLua.GetVariable("Quest1.TotalSlots").asInt;
        }

        private void AlertaMissaoCompleta()
        {
            DialogueManager.ShowAlert("Parabéns!");
            playerStatus.Comemorar();
            DialogueManager.ShowAlert(
                "Avise sua mãe que você guardou todos os brinquedos!"
            );
            QuestLog.SetQuestDescription(
                QuestsEnum.GuardarBrinquedos,
                QuestState.ReturnToNPC,
                "Avise sua mãe que você guardou todos os brinquedos!"
            );
        }

        private void PlayPlaceItemFeedback(Transform emptySlot)
        {
            if (PlaceItemFeedback != null)
            {
                PlaceItemFeedback.transform.position = emptySlot.position;
                PlaceItemFeedback.PlayFeedbacks();
            }
        }

        private void ProgredirMissao()
        {
            string totalToysOnShelfName = QuestsEnum.QtdItensGuardadosNoArmario;
            DialogueLua.SetVariable(
                totalToysOnShelfName,
                DialogueLua.GetVariable(totalToysOnShelfName).asInt + 1
            );
            DialogueManager.SendUpdateTracker();

        }

        private Transform ObtemEspacoDisponivel()
        {
            Transform espacoDisponivel = GetProximoEspacoDisponivel();
            if (espacoDisponivel != null)
            {
                var item = PlayerStatus.ItemCarregado;
                SetItemParentAndPosition(item, espacoDisponivel);
                PlayerStatus.MaoController.Soltar();
                return espacoDisponivel;
            }
            return null;
        }

        public void ColocarItemNoEspaco(GameObject item, Transform espaco)
        {
            SetItemParentAndPosition(item, espaco);
            item.GetComponent<PickableItem>().MarkAsCollected();
        }

        private void SetItemParentAndPosition(GameObject item, Transform slot)
        {
            item.transform.SetParent(slot);
            // coloca um item fake somente para que o espaço fique virutalmente ocupado
            var box = new GameObject("FakeItem");
            box.transform.SetParent(slot);
            item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }


        private Transform GetProximoEspacoDisponivel()
        {
            var espacosVazios = Slots.Where(s => s.childCount == 0).ToList();

            if (espacosVazios.Count > 0)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Transform espacoMaisProximoDaCamera = null;
                    float menorDistancia = float.MaxValue;

                    foreach (var espaco in espacosVazios)
                    {
                        float distancia = Vector3.Distance(hit.point, espaco.position);
                        if (distancia < menorDistancia)
                        {
                            menorDistancia = distancia;
                            espacoMaisProximoDaCamera = espaco;
                        }
                    }

                    return espacoMaisProximoDaCamera;
                }
            }
            return null;
        }
    }
}