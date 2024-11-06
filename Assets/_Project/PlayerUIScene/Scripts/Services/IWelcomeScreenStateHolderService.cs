using Core.ServiceLocator;
using UnityEngine.Events;

namespace PlayerUIScene.Services
{
    public interface IWelcomeScreenStateHolderService : IService
    {
        public bool IsWelcomeScreenVisible { get; set; }

        public UnityEvent OnOpenWelcomeScreen { get; set; }

        public UnityEvent OnDestroyWelcomeScreen { get; set; }

        void CheckIfScreenShouldBeVisible();
    }
}
