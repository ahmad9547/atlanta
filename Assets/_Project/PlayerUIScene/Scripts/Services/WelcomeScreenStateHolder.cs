using Core.ServiceLocator;
using LocationsManagement.Enums;
using LocationsManagement.Interfaces;
using UnityEngine.Events;

namespace PlayerUIScene.Services
{
    public sealed class WelcomeScreenStateHolder : IWelcomeScreenStateHolderService
    {
        private static bool _wasAlreadyShown;

        public bool IsWelcomeScreenVisible { get; set; }

        public UnityEvent OnOpenWelcomeScreen { get; set; } = new UnityEvent();

        public UnityEvent OnDestroyWelcomeScreen { get; set; } = new UnityEvent();

        public void CheckIfScreenShouldBeVisible()
        {
            if (_wasAlreadyShown)
            {
                OnDestroyWelcomeScreen?.Invoke();
                return;
            }

            OpenWelcomeScreen();
        }

        private void OpenWelcomeScreen()
        {
            OnOpenWelcomeScreen?.Invoke();
            IsWelcomeScreenVisible = true;
            _wasAlreadyShown = true;
        }
    }
}