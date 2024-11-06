using Core.ServiceLocator;
using PlayerUIScene.SideMenu;
using UnityEngine.Events;

namespace _Project.PlayerUIScene.Scripts.SideMenu.Services
{
    public interface ISideMenuService : IService
    {
        UnityEvent ChangeMenuPositionToClosedEvent { get; set; }
        UnityEvent<MenuPosition> ChangeMenuPositionEvent { get; set; }

        void ChangeMenuPositionToClosed();
        void ChangeMenuPosition(MenuPosition targetMenuPosition);
        void LockCursor();
        void UnlockCursor();
    }
}