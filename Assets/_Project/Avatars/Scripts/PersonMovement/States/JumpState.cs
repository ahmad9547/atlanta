using Avatars.PersonMovement.MoveSettings;
using UnityEngine;

namespace Avatars.PersonMovement.States
{
    public sealed class JumpState : MovementState
    {
        private const float RigidbodyJumpForceMultiplier = 150f;

        private JumpSettings _jumpSettings;        

        public JumpState(PlayerMovement playerMovement, JumpSettings jumpSettings) : base(playerMovement)
        {
            _jumpSettings = jumpSettings;
        }

        public override void Enter()
        {
            _playerAnimator.SetJumpParameter();
            ApplyMovement();
        }

        private void ApplyMovement()
        {
            _rigidbody.AddForce(_playerBody.up * _jumpSettings.JumpHeight * RigidbodyJumpForceMultiplier, ForceMode.Impulse);
        }
    }
}
