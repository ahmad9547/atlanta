using Avatars.PersonMovement.Services;
using Avatars.WebGLMovement.MouseControll.Interfaces;
using Common.PlayerInput.Interfaces;
using Core.ServiceLocator;
using Core.UI;
using PlayerUIScene.Services;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUIScene.GiftShopMenu.Popups
{
    public class Popup : UIController
    {
        [SerializeField] private Button _screenFreeSpaceButton;
        [SerializeField] protected Button _closeButton;

        #region Services

        private IMouseCursorService _mouseCursorInstance;
        private IMouseCursorService _mouseCursor
            => _mouseCursorInstance ??= Service.Instance.Get<IMouseCursorService>();

        private IPlayerMovementLockerService _playerMovementLockerInstance;
        private IPlayerMovementLockerService _playerMovementLocker
            => _playerMovementLockerInstance ??= Service.Instance.Get<IPlayerMovementLockerService>();

        private IMenuService _menuServiceInstance;
        private IMenuService _menuService
            => _menuServiceInstance ??= Service.Instance.Get<IMenuService>();

        private IPlayerInputEventHandler _playerInputEventHandlerInstance;
        private IPlayerInputEventHandler _playerInputEventHandler
            => _playerInputEventHandlerInstance ??= Service.Instance.Get<IPlayerInputEventHandler>();

        private IWelcomeScreenStateHolderService _welcomeScreenStateHolderInstance;
        private IWelcomeScreenStateHolderService _welcomeScreenStateHolder
            => _welcomeScreenStateHolderInstance ??= Service.Instance.Get<IWelcomeScreenStateHolderService>();

        #endregion

        protected virtual void OnEnable()
        {
            _screenFreeSpaceButton.onClick.AddListener(ClosePopup);
            _closeButton.onClick.AddListener(ClosePopup);
            _playerInputEventHandler.OnSideMenuSwitch.AddListener(ClosePopup);
        }

        protected virtual void OnDisable()
        {
            _screenFreeSpaceButton.onClick.RemoveListener(ClosePopup);
            _closeButton.onClick.RemoveListener(ClosePopup);
            _playerInputEventHandler.OnSideMenuSwitch.RemoveListener(ClosePopup);
        }

        protected void ShowPopup()
        {
            _menuService.CloseSideMenu();
            _playerMovementLocker.SetMovementLockState(true);
            _mouseCursor.UnlockCursor();
            Show();
        }

        protected void ClosePopup()
        {
            Hide();

            if (_welcomeScreenStateHolder.IsWelcomeScreenVisible)
            {
                return;
            }

            if (_menuService.IsSideMenuOpen)
            {
                return;
            }

            _mouseCursor.LockCursor();
            _playerMovementLocker.SetMovementLockState(false);
        }
    }
}