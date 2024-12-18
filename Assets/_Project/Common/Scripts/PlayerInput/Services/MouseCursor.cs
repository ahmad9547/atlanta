using Avatars.WebGLMovement.MouseControll.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Common.PlayerInput.Services
{
    public sealed class MouseCursor : IMouseCursorService
    {
        public UnityEvent<bool> OnMouseCursorStateChanged { get; } = new UnityEvent<bool>();

        public bool IsLocked { get; private set; }

        public void LockCursor()
        {
            IsLocked = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            OnMouseCursorStateChanged?.Invoke(true);
        }

        public void UnlockCursor()
        {
            IsLocked = false;

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            OnMouseCursorStateChanged?.Invoke(false);
        }
    }
}
