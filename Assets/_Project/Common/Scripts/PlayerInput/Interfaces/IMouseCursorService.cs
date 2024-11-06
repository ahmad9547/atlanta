using Core.ServiceLocator;
using UnityEngine.Events;

namespace Avatars.WebGLMovement.MouseControll.Interfaces
{
    public interface IMouseCursorService : IService
    {
        UnityEvent<bool> OnMouseCursorStateChanged { get; }

        bool IsLocked { get; }

        void LockCursor();
        void UnlockCursor();
    }
}