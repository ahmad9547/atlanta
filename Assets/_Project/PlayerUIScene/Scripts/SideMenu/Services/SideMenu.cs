using Avatars.PersonMovement.Services;
using Avatars.WebGLMovement.MouseControll.Interfaces;
using Core.ServiceLocator;
using PlayerUIScene.Services;
using PlayerUIScene.SideMenu;
using UnityEngine.Events;

namespace _Project.PlayerUIScene.Scripts.SideMenu.Services
{
    public class SideMenu : ISideMenuService
    {
        public UnityEvent ChangeMenuPositionToClosedEvent { get; set; } = new UnityEvent();
        public UnityEvent<MenuPosition> ChangeMenuPositionEvent { get; set; } = new UnityEvent<MenuPosition>();

        #region Services

        private IPlayerMovementLockerService _playerMovementLockerInstance;
        private IPlayerMovementLockerService _playerMovementLocker
            => _playerMovementLockerInstance ??= Service.Instance.Get<IPlayerMovementLockerService>();

        private IMouseCursorService _mouseCursorInstance;
        private IMouseCursorService _mouseCursor
            => _mouseCursorInstance ??= Service.Instance.Get<IMouseCursorService>();

        private IMenuService _menuServiceInstance;
        private IMenuService _menuService
            => _menuServiceInstance ??= Service.Instance.Get<IMenuService>();

        #endregion

        public void ChangeMenuPositionToClosed()
        {
            ChangeMenuPositionToClosedEvent?.Invoke();
        }

        public void ChangeMenuPosition(MenuPosition targetMenuPosition)
        {
            ChangeMenuPositionEvent?.Invoke(targetMenuPosition);
        }

        public void LockCursor()
        {
            _mouseCursor.LockCursor();
            _playerMovementLocker.SetMovementLockState(false);
            _menuService.IsSideMenuOpen = false;
        }

        public void UnlockCursor()
        {
            _mouseCursor.UnlockCursor();
            _playerMovementLocker.SetMovementLockState(true);
            _menuService.IsSideMenuOpen = true;
        }
    }
}