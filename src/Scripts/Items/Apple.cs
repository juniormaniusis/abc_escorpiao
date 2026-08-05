using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Player;
using MoreMountains.Feedbacks;
using UnityEngine;
using DG.Tweening;

namespace Assets.Scripts.Items
{
    public class Apple : MonoBehaviour
    {

        public MMF_Player OnFloorFeedback;
        public MMF_Player PickUpFeedback;
        public int Pontos = 1;
        private static PlayerStatus PlayerStatus => PlayerStatus.Instance;

        void Start()
        {
            OnFloorFeedback.PlayFeedbacks();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GetComponent<Collider>().enabled = false;
                PontuacaoController.Instance.AdicionarPontos(Pontos, true);
                PickUpFeedback.PlayFeedbacks();
            }
        }
    }
}