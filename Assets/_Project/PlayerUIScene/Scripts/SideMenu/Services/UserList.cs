using Core.ServiceLocator;
using Photon.Pun;
using UnityEngine.Events;
using UserManagement;

namespace _Project.PlayerUIScene.Scripts.SideMenu.Services
{
    public class UserList : IUserListService
    {
        public UnityEvent UpdatePlayersEvent { get; } = new UnityEvent();
        public UnityEvent<int> UpdatePlayerUserItemEvent { get; } = new UnityEvent<int>();

        #region Services

        private IUserProfileService _userProfileService;
        private IUserProfileService _userProfile
            => _userProfileService ??= Service.Instance.Get<IUserProfileService>();

        #endregion

        public PlayersMenuUserModel InitializeLocalPlayerItem()
        {
            return new PlayersMenuUserModel()
            {
                Nickname = _userProfile.LocalUserModel.Nickname,
                AvatarId = _userProfile.LocalUserModel.AvatarId,
                IsAdmin = _userProfile.LocalUserModel.IsAdmin,
                ActorNumber = PhotonNetwork.LocalPlayer.ActorNumber,
                PhotonPlayer = PhotonNetwork.LocalPlayer
            };
        }
        public void UpdatePlayers()
        {
            UpdatePlayersEvent?.Invoke();
        }

        public void UpdatePlayerUserItem(int playerActorNumber)
        {
            UpdatePlayerUserItemEvent?.Invoke(playerActorNumber);
        }
    }
}