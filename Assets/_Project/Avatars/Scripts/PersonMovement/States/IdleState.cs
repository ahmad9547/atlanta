namespace Avatars.PersonMovement.States
{
    public sealed class IdleState : MovementState
    {
        public IdleState(PlayerMovement playerMovement) : base(playerMovement) { }

        public override void Enter()
        {
            _playerMovement.PlayerAnimator.ResetAnimator();
        }
    }
}
