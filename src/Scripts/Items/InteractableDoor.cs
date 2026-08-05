using PixelCrushers.DialogueSystem.Wrappers;
using UnityEngine;
using PixelCrushers;
using Assets.Scripts.GameManager;
using Assets.Scripts.Player;
using MoreMountains.Feedbacks;
using PixelCrushers.DialogueSystem;
using Usable = PixelCrushers.DialogueSystem.Wrappers.Usable;
using System.Linq;

namespace Assets.Scripts.Items
{
    public class InteractableDoor : Usable, IInteractable
    {
        public ObjectType ObjectType { get; set; }
        // create a condition to check if the door is locked
        // if locked, play a feedback to indicate that the door is locked
        // if unlocked, play a feedback to indicate that the door is unlocked

        [SerializeField] private PixelCrushers.DialogueSystem.Condition OpenCondition;
        [SerializeField] private string destinationSceneName;
        [SerializeField] private string spawnpointNameInDestinationScene;
        private bool isLoadingScene;

        [SerializeField] private MMF_Player onUseFeedback;
        [SerializeField] private MMF_Player onUseLockedFeedback;
        [SerializeField] private string onUseLockedMessage;
        [SerializeField] private float interactionDistance = 3f;
        private SphereCollider interactionCollider;



        new void Start()
        {
            ObjectType = ObjectType.Door;
            events.onUse.AddListener
            (
                () =>
                {
                    Interact(PlayerStatus.Instance.transform);
                }
            );
            events.onSelect.AddListener(OnSelect);


            // verifica se tem collider com trigger, se não tiver, cria um
            var colliders = GetComponents<Collider>();
            var someHasTrigger = colliders.Any(c => c.isTrigger);
            if (!someHasTrigger)
            {
                interactionCollider = gameObject.AddComponent<SphereCollider>();
                interactionCollider.isTrigger = true;
                interactionCollider.radius = interactionDistance;
                interactionCollider.center = Vector3.zero;
            }

        }

        private void OnSelect()
        {
            if (OpenCondition == null)
                return;

            if (OpenCondition.IsTrue(transform))
            {
                overrideUseMessage = string.Empty;
            }
            else
            {
                overrideUseMessage = "A porta está trancada.";
            }
        }

        public void Interact(Transform actor)
        {
            if (OpenCondition == null || isLoadingScene)
                return;

            if (!OpenCondition.IsTrue(transform))
            {
                if (onUseLockedFeedback != null)
                {
                    onUseLockedFeedback.PlayFeedbacks();
                    DialogueManager.ShowAlert(onUseLockedMessage);
                }
                return;
            }

            if (onUseFeedback != null)
            {
                isLoadingScene = true;
                DontDestroyOnLoad(onUseFeedback.gameObject);
                // Só carrega a cena quando o feedback terminar
                onUseFeedback.Events.OnComplete.AddListener(LoadDestinationScene);
                // Destroi o feedback depois de carregar a cena
                onUseFeedback.Events.OnComplete.AddListener(() => Destroy(onUseFeedback.gameObject));
                onUseFeedback.PlayFeedbacks();
            }
            else
            {
                LoadDestinationScene();
            }
        }

        private void LoadDestinationScene()
        {
            onUseFeedback.Events.OnComplete.RemoveListener(LoadDestinationScene); // Evita múltiplas chamadas
            SceneTransition.Load(
                string.IsNullOrEmpty(spawnpointNameInDestinationScene)
                ? destinationSceneName
                : destinationSceneName + "@" + spawnpointNameInDestinationScene
            );
        }
    }
}

