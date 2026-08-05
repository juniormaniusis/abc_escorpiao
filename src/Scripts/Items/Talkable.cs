using Assets.Scripts.NPC;
using Assets.Scripts.Player;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using KinematicCharacterController;
using System.Collections;

namespace Assets.Scripts.Items
{
    [RequireComponent(typeof(NPCAnimator))]
    public class Talkable : Usable
    {
        private const string DisableCinemachineLua = "";
        private const string EnableCinemachineLua = "";

        private ObjectType ObjectType { get; set; }
        public DialogueSystemTrigger OnUse { get; private set; }
        public DialogueSystemTrigger OnConversationEnd { get; private set; }
        public DialogueDatabase dialogueDatabase;

        [Tooltip("Use this actor name in conversations.")]
        [ActorPopup(true)]
        [UnityEngine.Serialization.FormerlySerializedAs("overrideName")]
        public string actor;

        [SerializeField] private float interactionDistance = 3f;


        private DialogueActor dialogueActor;

        [ConversationPopup(false, true)]
        public string conversation = string.Empty;

        private NPCAnimator npcAnimator;
        private Coroutine _alignmentCoroutine;

        // Instead of storing a reference to a Transform (which points to the same object),
        // we save the world position and rotation values so we keep a copy.
        Vector3 _originalPosition;
        Quaternion _originalRotation;
        bool _hasOriginalPosition = false;

        #region Lua registration
        void OnEnable()
        {
            Lua.RegisterFunction(
                "NpcTalking",
                this,
                SymbolExtensions.GetMethodInfo(() => NpcTalking(true))
            );



            events.onUse.AddListener(AlinharTalkableComPlayer);
            events.onDeselect.AddListener(RetornarPosicaoOriginal);

        }

        private void AlinharTalkableComPlayer()
        {
            if (_alignmentCoroutine != null) StopCoroutine(_alignmentCoroutine);
            _alignmentCoroutine = StartCoroutine(RotateTowardsPlayer());
        }

        private void RetornarPosicaoOriginal()
        {
            if (_alignmentCoroutine != null) StopCoroutine(_alignmentCoroutine);
            _alignmentCoroutine = StartCoroutine(ReturnToOriginalTransform());
        }

        private IEnumerator RotateTowardsPlayer()
        {
            if (PlayerStatus.Instance == null || PlayerStatus.Instance.MovimentoController == null) yield break;

            var player = PlayerStatus.Instance.MovimentoController.transform;
            var targetDirection = (player.position - transform.position);
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero) yield break;

            var targetRotation = Quaternion.LookRotation(targetDirection);
            float time = 0;
            float duration = 0.5f;
            Quaternion startRotation = transform.rotation;

            while (time < duration)
            {
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            transform.rotation = targetRotation;
        }

        private IEnumerator ReturnToOriginalTransform()
        {
            if (!_hasOriginalPosition) yield break;

            float time = 0;
            float duration = 0.5f;
            Quaternion startRotation = transform.rotation;
            Vector3 startPosition = transform.position;

            while (time < duration)
            {
                float t = time / duration;
                transform.rotation = Quaternion.Slerp(startRotation, _originalRotation, t);
                transform.position = Vector3.Lerp(startPosition, _originalPosition, t);
                time += Time.deltaTime;
                yield return null;
            }
            transform.rotation = _originalRotation;
            transform.position = _originalPosition;
        }

        new void OnDisable()
        {
            Lua.UnregisterFunction("NpcTalking");
        }

        #endregion

        public void NpcTalking(bool talking)
        {
            npcAnimator.conversando = talking;
            Debug.Log("NpcTalking: " + talking);
        }

        public new void Start()
        {
            if (!_hasOriginalPosition)
            {
                _originalPosition = transform.position;
                _originalRotation = transform.rotation;
                _hasOriginalPosition = true;
            }

            ObjectType = ObjectType.Dialog;
            base.Start();

            npcAnimator = GetComponent<NPCAnimator>();
            var triggerCollider = gameObject.AddComponent<BoxCollider>();
            triggerCollider.isTrigger = true;
            triggerCollider.size = new Vector3(interactionDistance, 1, interactionDistance);

            InitializeDialogueActor();
            InitializeDialogueSystemTriggers();
        }

        private void InitializeDialogueActor()
        {
            dialogueActor = gameObject.AddComponent<DialogueActor>();
            dialogueActor.actor = actor;
        }

        private void InitializeDialogueSystemTriggers()
        {
            foreach (var item in GetComponents<DialogueSystemTrigger>())
            {
                if (item.trigger == DialogueSystemTriggerEvent.OnUse)
                {
                    OnUse = item;
                }
                else if (item.trigger == DialogueSystemTriggerEvent.OnConversationEnd)
                {
                    OnConversationEnd = item;
                }
            }

            if (OnUse == null)
            {
                OnUse = CreateDialogueSystemTriggerOnUse();
            }

            if (OnConversationEnd == null)
            {
                OnConversationEnd = CreateDialogueSystemTriggerOnConversationEnd();
            }
        }

        private DialogueSystemTrigger CreateDialogueSystemTriggerOnUse()
        {
            var trigger = gameObject.AddComponent<DialogueSystemTrigger>();
            trigger.trigger = DialogueSystemTriggerEvent.OnUse;
            trigger.selectedDatabase = dialogueDatabase;
            trigger.conversation = conversation;
            trigger.luaCode = DisableCinemachineLua;
            return trigger;
        }

        private DialogueSystemTrigger CreateDialogueSystemTriggerOnConversationEnd()
        {
            var trigger = gameObject.AddComponent<DialogueSystemTrigger>();
            trigger.trigger = DialogueSystemTriggerEvent.OnConversationEnd;
            trigger.selectedDatabase = dialogueDatabase;
            trigger.luaCode = EnableCinemachineLua;
            return trigger;
        }


    }
}