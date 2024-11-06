using Avatars.PersonMovement.Services;
using Avatars.WebGLMovement.MouseControll.Interfaces;
using Core.ServiceLocator;
using Core.UI;
using LocationsManagement.Enums;
using Metaverse.ErrorHandling.Services;
using PlayerUIScene.Services;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUIScene.WelcomeMenu
{
    [DefaultExecutionOrder(1000)]
    public sealed class WelcomeScreen : UIController
    {
        private static bool _wasAlreadyShown = false;

        [SerializeField] private Button _closeButton;

        #region Services

        private IPlayerMovementLockerService _playerMovementLockerInstance;
        private IPlayerMovementLockerService _playerMovementLocker
            => _playerMovementLockerInstance ??= Service.Instance.Get<IPlayerMovementLockerService>();

        private IMouseCursorService _mouseCursorInstance;
        private IMouseCursorService _mouseCursor
            => _mouseCursorInstance ??= Service.Instance.Get<IMouseCursorService>();

        private IWelcomeScreenStateHolderService _welcomeScreenStateHolderInstance;
        private IWelcomeScreenStateHolderService _welcomeScreenStateHolder
            => _welcomeScreenStateHolderInstance ??= Service.Instance.Get<IWelcomeScreenStateHolderService>();

        private IErrorHandlingService _errorHandlingService;
        private IErrorHandlingService ErrorHandlingService =>
            _errorHandlingService ??= Service.Instance.Get<IErrorHandlingService>();

        #endregion

        private readonly LocationType _locationTypeWhenScreenIsVisible = LocationType.OlympicPark;

        protected override void Start()
        {
            _welcomeScreenStateHolder.CheckIfScreenShouldBeVisible();
        }

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(CloseWelcomeScreen);
            _welcomeScreenStateHolder.OnOpenWelcomeScreen.AddListener(OpenWelcomeScreen);
            _welcomeScreenStateHolder.OnDestroyWelcomeScreen.AddListener(DestroyWelcomeScreen);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(CloseWelcomeScreen);
            _welcomeScreenStateHolder.OnOpenWelcomeScreen.RemoveListener(OpenWelcomeScreen);
            _welcomeScreenStateHolder.OnDestroyWelcomeScreen.RemoveListener(DestroyWelcomeScreen);
        }

        private void CloseWelcomeScreen()
        {
            _uiAnimator.ButtonScale(_closeButton, () =>
            {
                _mouseCursor.LockCursor();
                _playerMovementLocker.SetMovementLockState(false);
                _welcomeScreenStateHolder.IsWelcomeScreenVisible = false;
                Hide();
            });
        }

        private void OpenWelcomeScreen()
        {
            _playerMovementLocker.SetMovementLockState(true);
            _mouseCursor.UnlockCursor();

            if (ErrorHandlingService.IsErrorPopupOpen)
            {
                ErrorHandlingService.OnErrorPopupClosed.AddListener(OpenScreen);
                return;
            }

            OpenScreen();
        }

        private void DestroyWelcomeScreen()
        {
            Destroy(gameObject);
        }

        private void OpenScreen()
        {
            ErrorHandlingService.OnErrorPopupClosed.RemoveListener(OpenScreen);
            Show();
        }
    }
}