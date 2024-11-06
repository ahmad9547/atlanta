using UnityEngine.Events;

namespace Avatars.PersonMovement.Services
{
    public sealed class PlayerMovementLocker : IPlayerMovementLockerService
    {
        public UnityEvent<bool> OnMovementLockedStateChanged { get; } = new UnityEvent<bool>();


        public void SetMovementLockState(bool lockMovement)
        {
            OnMovementLockedStateChanged?.Invoke(lockMovement);
        }
    }
}