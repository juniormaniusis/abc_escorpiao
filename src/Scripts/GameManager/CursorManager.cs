using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Assets.Scripts.GameManager
{
    public class CursorManager : MonoBehaviour
    {
        private bool _isCursorLocked = true;

        void Start()
        {
            // UpdateCursorLock();
        }

        void OnApplicationFocus(bool hasFocus)
        {
            // if (hasFocus && _isCursorLocked)
            // {
            //     UpdateCursorLock();
            // }
        }

        void Update()
        {
            // if (DialogueManager.instance.isConversationActive || Input.GetKeyDown(KeyCode.Escape))
            // {
            //     Cursor.lockState = CursorLockMode.None;
            //     Cursor.visible = true;
            //     _isCursorLocked = false;
            // }
            // else if (_isCursorLocked != true)
            // {
            //     UpdateCursorLock();
            // }
        }

        private void UpdateCursorLock()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _isCursorLocked = true;
        }
    }
}