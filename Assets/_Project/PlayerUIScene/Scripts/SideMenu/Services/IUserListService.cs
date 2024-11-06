using Core.ServiceLocator;
using UnityEngine.Events;
using UserManagement;

namespace _Project.PlayerUIScene.Scripts.SideMenu.Services
{
    public interface IUserListService : IService
    {
        UnityEvent UpdatePlayersEvent { get; }
        UnityEvent<int> UpdatePlayerUserItemEvent { get; }

        PlayersMenuUserModel InitializeLocalPlayerItem();
        void UpdatePlayers();
        void UpdatePlayerUserItem(int playerActorNumber);
    }
}