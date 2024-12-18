using Core.ServiceLocator;
using UnityEngine.Events;

namespace PlayerUIScene.Services
{
    public interface IMenuService : IService
    {
        bool IsSideMenuOpen { get; set; }

        UnityEvent OnSideMenuClosed { get; }

        void CloseSideMenu();
    }
}