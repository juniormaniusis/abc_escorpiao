using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(PlayerStatus))]
    public class PlayerCameraController : MonoBehaviour
    {
        public static event System.Action<GameObject> OnPlayerCameraReady;

        public GameObject Camera { get; private set; }

        public static void RaisePlayerCameraReady(GameObject cameraObj)
        {
            OnPlayerCameraReady?.Invoke(cameraObj);
        }

        void Start()
        {
            OnPlayerCameraReady += HandlePlayerCameraReady;
        }

        private void InicializaCamera()
        {
            Camera = GameObject.FindGameObjectWithTag("VCAM_PLAYER");
            if (Camera == null)
            {
                Debug.LogError("VCAM_PLAYER not found in the scene.");
                return;
            }

            if (Camera.TryGetComponent<TMPro.Examples.CameraController>(out var cameraController))
            {
                cameraController.SetCameraTarget();
            }
            else
            {
                Debug.LogWarning("CameraController component not found on VCAM_PLAYER.");
            }
        }

        private void HandlePlayerCameraReady(GameObject cameraObj)
        {
            Camera = cameraObj;
            if (Camera.TryGetComponent<TMPro.Examples.CameraController>(out var cameraController))
            {
                cameraController.SetCameraTarget();
            }
            else
            {
                Debug.LogWarning("CameraController component not found on VCAM_PLAYER.");
            }
        }
    }
}