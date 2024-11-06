using Core.ServiceLocator;
using UnityEngine.Events;

namespace Avatars.PersonMovement.Services
{
    public interface IPlayerMovementLockerService : IService
    {
        UnityEvent<bool> OnMovementLockedStateChanged { get; }

        void SetMovementLockState(bool lockMovement);
    }
}