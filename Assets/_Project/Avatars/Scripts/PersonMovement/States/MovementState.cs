using UnityEngine;

namespace Avatars.PersonMovement.States
{
    public abstract class MovementState
    {
        protected readonly PlayerMovement _playerMovement;

        protected Transform _playerBody => _playerMovement.PlayerBody;

        protected PlayerMovementInput _movementInput => _playerMovement.PlayerMovementInput;

        protected Rigidbody _rigidbody => _playerMovement.PlayerRigidbody;

        protected IPlayerAnimator _playerAnimator => _playerMovement.PlayerAnimator;

        protected MovementState(PlayerMovement playerMovement)
        {
            _playerMovement = playerMovement;
        }

        public virtual void Enter() { }

        public virtual void Update() { }

        public virtual void FixedUpdate() { }

        public virtual void Exit() { }
    }
}
