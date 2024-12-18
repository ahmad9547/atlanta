using UnityEngine;
using Core.ServiceLocator;
using Photon.Pun;
using PhotonEngine.CustomProperties;
using PhotonEngine.CustomProperties.Enums;

namespace UserManagement
{
    public sealed class UserProfile : IUserProfileService
    {
        private UserModel _localUserModel;
        private string _avatarLink;
        private string _nickname;
        private bool _isAdmin;

        public string AvatarLink => _avatarLink;
        public string Nickname => _nickname;
        public UserModel LocalUserModel => _localUserModel;
        public bool IsAdmin => _isAdmin;

        #region Services

        private IPlayerCustomPropertiesService _playerCustomPropertiesService;
        private IPlayerCustomPropertiesService _playerCustomProperties
            => _playerCustomPropertiesService ??= Service.Instance.Get<IPlayerCustomPropertiesService>();

        #endregion

        public void SetupUserProfile(UserModel userModel)
        {
            if (userModel == null)
            {
                Debug.LogError("UserModel reference for setup is null");
                return;
            }

            _localUserModel = userModel;

            SetUserNickname(userModel.Nickname);
            SetUserAvatar(userModel.AvatarId);
            _isAdmin = userModel.IsAdmin;
        }

        private void SetUserAvatar(string avatarId)
        {
            _avatarLink = avatarId;
            _playerCustomProperties.AddOrUpdateLocalPlayerCustomProperty(PlayerCustomPropertyKey.AvatarLink, avatarId);
        }

        private void SetUserNickname(string nickname)
        {
            _nickname = nickname;
            PhotonNetwork.LocalPlayer.NickName = nickname;
        }
    }
}