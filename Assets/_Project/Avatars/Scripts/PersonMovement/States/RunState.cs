using Avatars.PersonMovement.MoveSettings;
using UnityEngine;

namespace Avatars.PersonMovement.States
{
    public sealed class RunState : MovementState
    {
        private const float RigidbodyRunSpeedMultiplier = 50f;

        private RunSettings _runSettings;
        
        public RunState(PlayerMovement playerMovement, RunSettings runSettings) : base(playerMovement)
        {
            _runSettings = runSettings;
        }

        public override void Enter()
        {
            _playerAnimator.SetRunParameter(true);
        }

        public override void FixedUpdate()
        {
            if (!_playerMovement.IsGrounded)
            {
                return;
            }

            ApplyMovement();
        }

        public override void Exit()
        {
            _playerAnimator.SetRunParameter(false);
        }

        private void ApplyMovement()
        {
            Vector3 moveDirection = _playerBody.right * _movementInput.HorizontalInput + _playerBody.forward * _movementInput.VerticalInput;
            Vector3 moveVelocity = moveDirection * _runSettings.RunSpeed * Time.fixedDeltaTime * RigidbodyRunSpeedMultiplier;

            _playerMovement.PlayerRigidbody.velocity = 
                new Vector3(moveVelocity.x,
                _playerMovement.PlayerRigidbody.velocity.y,
                moveVelocity.z);
        }
    }
}
