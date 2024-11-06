using UnityEngine.Events;

namespace PlayerUIScene.Services
{
    public sealed class MenuService : IMenuService
    {
        public bool IsSideMenuOpen { get; set; }

        public UnityEvent OnSideMenuClosed { get; } = new UnityEvent();

        public void CloseSideMenu()
        {
            OnSideMenuClosed?.Invoke();
        }
    }
}