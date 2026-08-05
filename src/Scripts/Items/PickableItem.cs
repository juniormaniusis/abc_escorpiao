using Assets.Scripts.Player;
using MoreMountains.Feedbacks;
using PixelCrushers.DialogueSystem.Wrappers;
using UnityEngine;


namespace Assets.Scripts.Items
{
    [RequireComponent(typeof(SphereCollider))]
    public class PickableItem : MonoBehaviour, ICollectable
    {
        private PlayerStatus PlayerStatus => PlayerStatus.Instance;

        [SerializeField] private MMF_Player NotCollectedFeedbacksPrefab;
        [SerializeField] private MMF_Player SelectedFeedbacksPrefab;
        [SerializeField] private MMF_Player DeselectedFeedbacksPrefab;
        private MMF_Player NotCollectedFeedbacks;
        private MMF_Player SelectedFeedbacks;
        private MMF_Player DeselectedFeedbacks;
        public ObjectType ObjectType;

        #region Gizmos
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            if (TryGetComponent<SphereCollider>(out var sphereCollider))
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawWireSphere(sphereCollider.center, sphereCollider.radius);
            }
        }


        #endregion

        private bool _isCollected;
        public bool IsCollected
        {
            get => _isCollected;
            set
            {
                if (_isCollected != value)
                {
                    _isCollected = value;
                    if (_isCollected)
                    {
                        MarkAsCollected();
                    }
                }
            }
        }
        private void Awake()
        {
            if (NotCollectedFeedbacksPrefab == null
                || SelectedFeedbacksPrefab == null
                || DeselectedFeedbacksPrefab == null)
            {
                Debug.LogError("Feedbacks prefabs not set in PickableItem");
                return;
            }

            if (IsCollected) return;

            InitializeFeedbacks();
        }

        private void InitializeFeedbacks()
        {
            NotCollectedFeedbacks = Instantiate(NotCollectedFeedbacksPrefab, transform);
            SelectedFeedbacks = Instantiate(SelectedFeedbacksPrefab, transform);
            DeselectedFeedbacks = Instantiate(DeselectedFeedbacksPrefab, transform);
        }

        public void Start()
        {
            PlayNotCollectedFeedbacks();
        }

        private void PlayNotCollectedFeedbacks()
        {
            if (!IsCollected)
                NotCollectedFeedbacks.PlayFeedbacks();
        }

        public void Drop()
        {
            SetCollidersEnabled(true);
            RemoverItemDaMaoDoJogador();
        }

        public void Collect()
        {
            if (IsCollected || PlayerStatus.EstaCarregandoItem)
            {
                return;
            }

            IsCollected = true;
            SetCollidersEnabled(false);
            ColocarItemNaMaoDoJogador();
        }

        public void OnSelect()
        {
            if (IsCollected) return;
            DeselectedFeedbacks.StopFeedbacks();
            SelectedFeedbacks.PlayFeedbacks();
        }

        public void OnDeselect()
        {
            if (IsCollected) return;
            SelectedFeedbacks.StopFeedbacks();
            DeselectedFeedbacks.PlayFeedbacks();
        }

        private void SetCollidersEnabled(bool enabled)
        {
            var colliders = gameObject.GetComponentsInChildren<Collider>();
            foreach (var collider in colliders)
            {
                collider.enabled = enabled;
            }
        }

        private void ColocarItemNaMaoDoJogador()
        {
            PlayerStatus.MaoController.Segurar(gameObject);
        }

        private void RemoverItemDaMaoDoJogador()
        {
            PlayerStatus.MaoController.Soltar();
        }

        bool ICollectable.CanCollect()
        {
            return true;
        }

        public void MarkAsCollected()
        {
            NotCollectedFeedbacks.StopFeedbacks();
            //remove o componente Usable do objeto
            Destroy(gameObject.GetComponent<Usable>());
            SelectedFeedbacks.StopFeedbacks();
        }
    }
}