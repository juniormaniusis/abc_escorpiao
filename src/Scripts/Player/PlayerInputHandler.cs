using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(PlayerStatus))]
    public class PlayerInputHandler : MonoBehaviour
    {
        private InputActions inputActions;
        public Vector2 MoveInput { get; private set; }
        public bool QuerExecutarAcao { get; private set; }

        private PlayerMovement movimentoController;
        private PlayerStatus playerStatus;
        private PlayerCameraController cameraController;

        // Base de direção travada no momento em que o jogador começa a andar.
        private Quaternion inputBasis = Quaternion.identity;
        private bool hasInputBasis;

        private const float MoveDeadzone = 0.1f;

        private void Awake()
        {
            playerStatus = GetComponent<PlayerStatus>();
            inputActions = new InputActions();
            inputActions.Enable();
            movimentoController = GetComponent<PlayerMovement>();
            cameraController = GetComponent<PlayerCameraController>();
        }

        private void Update()
        {
            // Early exit if application not focused or essential components are missing/destroyed
            if (!Application.isFocused || movimentoController == null || cameraController == null)
            {
                return;
            }

            MoveInput = inputActions.Game.Move.ReadValue<Vector2>();
            QuerExecutarAcao = inputActions.Game.Usar.IsPressed();

            // Check if the camera itself is available before trying to access its transform
            if (cameraController.Camera == null)
            {
                return;
            }

            // A base de direção é congelada enquanto a tecla estiver pressionada.
            // Sem isso, uma câmera que gira reinterpreta a mesma tecla a cada frame
            // e o personagem descreve uma espiral em vez de andar reto.
            float cameraYaw = cameraController.Camera.transform.eulerAngles.y;
            if (MoveInput.magnitude < MoveDeadzone)
            {
                hasInputBasis = false;
            }
            else if (!hasInputBasis)
            {
                inputBasis = Quaternion.Euler(0f, cameraYaw, 0f);
                hasInputBasis = true;
            }

            // Set input using the gathered values
            movimentoController.SetInput(new CharacterMovementInput
            {
                Move = MoveInput,
                WantsToUse = QuerExecutarAcao,
                LookRootation = hasInputBasis ? inputBasis : Quaternion.Euler(0f, cameraYaw, 0f)
            });
            playerStatus.QuerExecutarAcao = QuerExecutarAcao;
        }
    }
}