using Avatars.PersonMovement.MoveSettings;
using UnityEngine;

namespace Avatars.PersonMovement.States
{
    public sealed class FlyState : MovementState
    {
        private const float RigidbodyHorizontalFlyMultiplier = 50f;
        private const float RigidbodyVerticalFlyMultiplier = 40f;

        private FlySettings _flySettings;        

        public FlyState(PlayerMovement playerMovement, FlySettings flySettings) : base(playerMovement)
        {
            _flySettings = flySettings;
        }

        public override void Enter()
        {
            _playerAnimator.SetFlyParameter(true);
        }

        public static bool isHold = false;

        public override void FixedUpdate()
        {
            ApplyMovement();

        }

        public override void Exit()
        {
            _rigidbody.velocity = Vector3.zero;

            _playerAnimator.SetFlyParameter(false);
        }

        private void ApplyMovement()
        {
            Vector3 _moveDirection = _playerBody.right * _movementInput.HorizontalInput + _playerBody.forward * _movementInput.VerticalInput;

            Vector3 horizontalVelocityDirection = _moveDirection
                * _flySettings.FlyHorizontalSpeed
                * Time.fixedDeltaTime
                * RigidbodyHorizontalFlyMultiplier;

            float verticalVelocity = _flySettings.FlyVerticalSpeed * RigidbodyVerticalFlyMultiplier * Time.fixedDeltaTime;

            _rigidbody.velocity =
                new Vector3(horizontalVelocityDirection.x,
                verticalVelocity,
                horizontalVelocityDirection.z);
        }
    }
}
