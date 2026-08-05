using UnityEngine;
namespace Assets.Scripts.NPC
{
    public class NPCAnimator : MonoBehaviour
    {
        private Animator animator;
        public bool conversando;

        void Start()
        {
            animator = GetComponent<Animator>();
            SetIdleAnimation();
        }

        void Update()
        {
            if (conversando)
            {
                SetConversandoAnimation();
            }
            else
            {
                SetIdleAnimation();
            }
        }

        private void SetIdleAnimation()
        {
            animator.SetBool("isConversando", false);
        }

        private void SetConversandoAnimation()
        {
            animator.SetBool("isConversando", true);
        }
    }
}
