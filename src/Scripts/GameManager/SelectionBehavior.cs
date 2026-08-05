using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManager
{
    public class SelectionBehavior : MonoBehaviour
    {
        public GameObject Player;
        public float Threshold = 0.5f;
        public float RotationSpeed = 100f;
        public float YOffset = 0.5f;
        void Update()
        {
            if (Player == null)
            {
                Debug.LogError("Player is null");
                return;
            }
            Rotate();
            DeactivateIfBelowThreshold();
        }

        private void Rotate()
        {
            transform.Rotate(RotationSpeed * Time.deltaTime * Vector3.back);
        }
        private void DeactivateIfBelowThreshold()
        {

            if (gameObject.activeSelf &&
                (Player.transform.position - gameObject.transform.position).magnitude < Threshold)
            {
                DisableSelection();
            }
        }

        public void DisableSelection()
        {
            gameObject.SetActive(false);
        }

        public void CreateSelection(Vector3 point)
        {
            transform.position = point + Vector3.up * YOffset;
            gameObject.SetActive(true);
        }
    }
}