using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Assets.Scripts.Player;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Assets.Scripts.Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField]
        private bool SkipTutorial = false;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private MMF_Player StartTutorialFeedback;
        public bool TutorialDone => SkipTutorial || (cameraTutorialDone && playerMovementTutorialDone);

        #region CameraMovementTutorial
        private bool cameraTutorialDone = false;
        // [SerializeField]
        // private float cameraRequiredMovementTime = 2f;
        // private float cameraMovementTime = 0f;
        private bool tutorialStarted = false;

        private void CameraTutorial()
        {
            // if (!cameraTutorialDone)
            // {
            //     var mouseInput = playerMovement.Input.LookInput;
            //     if (mouseInput != null && mouseInput != UnityEngine.Vector2.zero)
            //     {
            //         cameraMovementTime += Time.deltaTime;
            //         if (cameraMovementTime >= cameraRequiredMovementTime)
            //         {
            //             cameraTutorialDone = true;
            //         }
            //     }
            // }
            // else PixelCrushers.DialogueSystem.Sequencer.Message(TutorialMessages.CameraMovement);
        }
        #endregion

        #region PlayerMovementTutorial
        private bool playerMovementTutorialDone = false;

        public void PlayerMovementTutorialDone()
        {
            playerMovementTutorialDone = true;
        }


        #endregion
        private void Start()
        {
            if (StartTutorialFeedback == null)
            {
                Debug.LogError("StartTutorialFeedback is null in TutorialManager");
            }

            if (SkipTutorial)
            {
                Debug.LogWarning("Tutorial skipped");
                cameraTutorialDone = true;
                playerMovementTutorialDone = true;
                tutorialStarted = true;
            }

        }
        private void Update()
        {

            // verify if TutorialManager is enabled
            if (!enabled) return;
            if (!tutorialStarted)
            {
                StartTutorialFeedback.PlayFeedbacks();
                tutorialStarted = true;
            }

            CameraTutorial();

            if (TutorialDone && !PixelCrushers.DialogueSystem.DialogueManager.IsConversationActive)
            {
                enabled = false;
            }
        }
    }
}