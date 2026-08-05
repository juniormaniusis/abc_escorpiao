using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Tutorial
{
    public class SendMessageOnPlayerTrigger : MonoBehaviour
    {
        public UnityEvent OnPlayerTrigger;
        public string message;

        void Start()
        {
            if (OnPlayerTrigger == null)
            {
                Debug.LogWarning("OnPlayerEnter is null in SendMessageOnPlayerTrigger");
            }
        }
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PixelCrushers.DialogueSystem.Sequencer.Message(message);
                OnPlayerTrigger?.Invoke();
            }
        }
    }
}