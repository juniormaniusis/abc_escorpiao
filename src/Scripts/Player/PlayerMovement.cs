using UnityEngine;
using KinematicCharacterController;
using System;
using PixelCrushers.DialogueSystem;

namespace Assets.Scripts.Player
{
    public struct CharacterMovementInput
    {
        public Vector2 Move;
        public Quaternion LookRootation;
        public bool WantsToJump;
        public Vector2 LookInput;

        public bool WantsToUse;
    }


    [RequireComponent(typeof(KinematicCharacterMotor))]
    public class PlayerMovement : MonoBehaviour, ICharacterController
    {
        private PlayerStatus PlayerStatus => PlayerStatus.Instance;
        public KinematicCharacterMotor motor;
        private Vector3 moveInput;
        [Header("Ground Movement")]
        public float MaxSpeed = 5f;
        [Range(0, 1)] public float SpeedReductionOnCarry = .4f;
        public float Acceleration = 50f;
        public float RotationSpeed = 15f;
        public float Gravity = 50f;

        [Header("Air Movement")]
        public float MaxAirSpeed = 3f;
        public float AirAcceleration = 20f;
        [Min(0)] public float Drag = 0.5f;
        private bool _allowMovement = true;
        public bool AllowMovement(bool value = true)
        {
            _allowMovement = value;
            return _allowMovement;
        }

        #region Lua registration
        void OnEnable()
        {
            Lua.RegisterFunction(
                "AllowMovement",
                this,
                SymbolExtensions.GetMethodInfo(() => AllowMovement(true))
            );
        }

        void OnDisable()
        {
            // Lua.UnregisterFunction("AllowMovement");
        }

        #endregion
        private void Awake()
        {
            motor.CharacterController = this;
        }

        public CharacterMovementInput Input { get; private set; }

        public void SetInput(CharacterMovementInput movementInput)
        {
            if (!_allowMovement) return;
            moveInput = Vector2.zero;
            if (movementInput.Move != Vector2.zero)
            {
                moveInput = new Vector3(movementInput.Move.x, 0, movementInput.Move.y);
                moveInput = movementInput.LookRootation * moveInput;
                moveInput.y = 0;
                moveInput.Normalize();
            }
            else
            {
                moveInput = Vector3.zero;
            }

            Input = movementInput;
        }


        internal bool IsMoving() => motor.Velocity.magnitude > 0.1f;

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (moveInput != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(moveInput);
                currentRotation = Quaternion.Slerp(currentRotation, targetRotation, RotationSpeed * deltaTime);
            }
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (motor.GroundingStatus.IsStableOnGround)
            {
                //TODO remove this "false"
                var targetVelocity = moveInput * (MaxSpeed * (false ? (1 - SpeedReductionOnCarry) : 1));
                currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, deltaTime * Acceleration);
            }
            else
            {
                var targetVelocityXZ = new Vector2(moveInput.x, moveInput.z) * MaxAirSpeed;
                var currentVelocityXZ = Vector2.MoveTowards(new Vector2(currentVelocity.x, currentVelocity.z), targetVelocityXZ, AirAcceleration * deltaTime);
                currentVelocity.x = ApplyDrag(currentVelocityXZ.x, Drag, deltaTime);
                currentVelocity.z = ApplyDrag(currentVelocityXZ.y, Drag, deltaTime);
                currentVelocity.y = -Gravity * deltaTime;

            }

        }

        private static float ApplyDrag(float v, float drag, float deltaTime)
        {
            return v * (1 / (1 + drag * deltaTime));
        }



        #region not implemented
        public void BeforeCharacterUpdate(float deltaTime)
        {

        }

        public void PostGroundingUpdate(float deltaTime)
        {

        }

        public void AfterCharacterUpdate(float deltaTime)
        {

        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {

        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {

        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {

        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {

        }
        #endregion
    }
}