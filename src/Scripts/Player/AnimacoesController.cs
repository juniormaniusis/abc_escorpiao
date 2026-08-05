
using System;
using Assets.Scripts.Player;
using UnityEngine;
namespace Assets.Scripts.Player
{
    public class AnimacoesController : MonoBehaviour
    {
        private Animator animator;
        private PlayerStatus playerStatus;
        private AudioSource audioSource;
        public AudioClip somComemoracao;

        void Start()
        {
            // get animator in child
            animator = GetComponentInChildren<Animator>();
            playerStatus = PlayerStatus.Instance;
            audioSource = GetComponent<AudioSource>();

        }

        void Update()
        {
            UpdateAnimator();
        }

        public void TrocarPersonagem(PersonagemEnum _)
        {
            animator = GetComponentInChildren<Animator>(includeInactive: false);
        }

        private void UpdateAnimator()
        {
            animator.SetBool("IsMoving", playerStatus.EstaMovendo);
            animator.SetBool("IsCleaning", playerStatus.ActionStatus == AcoesPossiveisEnum.Limpar);
        }

        internal void Comemorar()
        {
            animator.SetTrigger("Comemorar");
            TocarSomComemoracao();
        }

        private void TocarSomComemoracao()
        {
            audioSource.PlayOneShot(somComemoracao);
        }
    }
}