using Avatars.PersonMovement.MoveSettings;
using UnityEngine;

namespace Avatars.PersonMovement.States
{
    public sealed class HoldState : MovementState
    {
        public HoldState(PlayerMovement playerMovement) : base(playerMovement)
        {
        }

        public override void Enter()
        {
            _rigidbody.useGravity = false;
        }

        public override void FixedUpdate()
        {
            Vector3 _moveDirection = _playerBody.right * _movementInput.HorizontalInput + _playerBody.forward * _movementInput.VerticalInput;

            Vector3 horizontalVelocityDirection = _moveDirection
                * 0.5f
                * Time.fixedDeltaTime;

            _rigidbody.velocity =
                new Vector3(horizontalVelocityDirection.x,
                0,
                horizontalVelocityDirection.z);
        }

        public override void Exit()
        {
            _rigidbody.useGravity = true;

        }
    }
}
