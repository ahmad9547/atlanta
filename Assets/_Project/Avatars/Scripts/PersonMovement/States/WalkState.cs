using Avatars.PersonMovement.MoveSettings;
using UnityEngine;

namespace Avatars.PersonMovement.States
{
    public sealed class WalkState : MovementState
    {
        private const float RigidbodyWalkSpeedMultiplier = 50f;

        private WalkSettings _walkSettings;        

        public WalkState(PlayerMovement playerMovement, WalkSettings walkSettings) : base(playerMovement)
        {
            _walkSettings = walkSettings;
        }

        public override void Update()
        {
            if (!_playerMovement.IsGrounded)
            {
                return;
            }

            _playerAnimator.SetMovementParameters(_movementInput.HorizontalInput, _movementInput.VerticalInput);
        }

        public override void FixedUpdate()
        {
            if (!_playerMovement.IsGrounded)
            {
                return;
            }

            ApplyMovement();
        }

        private void ApplyMovement()
        {
            Vector3 moveDirection = _playerBody.right * _movementInput.HorizontalInput + _playerBody.forward * _movementInput.VerticalInput;
            Vector3 moveVelocity = moveDirection * _walkSettings.WalkSpeed * Time.fixedDeltaTime * RigidbodyWalkSpeedMultiplier;

            _playerMovement.PlayerRigidbody.velocity = 
                new Vector3(moveVelocity.x,
                _playerMovement.PlayerRigidbody.velocity.y,
                moveVelocity.z);
        }
    }
}
