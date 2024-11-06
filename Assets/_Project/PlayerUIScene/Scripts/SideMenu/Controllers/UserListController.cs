using Core.UI;
using PlayerUIScene.SideMenu.UserItems;
using UnityEngine;
using PhotonEngine.PhotonPlayers;
using PhotonEngine.PhotonPlayers.Interfaces;
using UserManagement;
using System.Collections.Generic;
using _Project.PlayerUIScene.Scripts.SideMenu.Services;
using Core.ServiceLocator;

namespace PlayerUIScene.SideMenu.Controllers
{
    public sealed class UserListController : UIController, IPhotonPlayersObserver
    {
        [Header("Items references:")]
        [SerializeField] private Transform _playersItemsParent;
        [SerializeField] private UserItem _userItemPrefab;
        [SerializeField] private AdminUserItem _adminUserItemPrefab;
        [SerializeField] private UserItem _localPlayerItem;

        private readonly Dictionary<int, UserItem> _playersUserItems = new Dictionary<int, UserItem>();

        #region Services

        private IUserProfileService _userProfileService;
        private IUserProfileService _userProfile
            => _userProfileService ??= Service.Instance.Get<IUserProfileService>();

        private IUserListService _userListInstance;
        private IUserListService _userList
            => _userListInstance ??= Service.Instance.Get<IUserListService>();

        private IPhotonRoomPlayersService _photonRoomPlayersInstance;
        private IPhotonRoomPlayersService _photonRoomPlayers
            => _photonRoomPlayersInstance ??= Service.Instance.Get<IPhotonRoomPlayersService>();

        #endregion

        private void OnEnable()
        {
            _photonRoomPlayers.AddPlayersObserver(this);
            _userList.UpdatePlayersEvent.AddListener(UpdatePlayers);
            _userList.UpdatePlayerUserItemEvent.AddListener(UpdatePlayerUserItem);
        }

        protected override void Start()
        {
            _localPlayerItem.InitializeItem(_userList.InitializeLocalPlayerItem());
        }

        private void OnDisable()
        {
            _photonRoomPlayers.RemovePlayersObserver(this);
            _userList.UpdatePlayersEvent.RemoveListener(UpdatePlayers);
            _userList.UpdatePlayerUserItemEvent.RemoveListener(UpdatePlayerUserItem);
        }

        public override void Show()
        {
            base.Show();
            UpdatePlayers();
        }

        public void UpdatePlayers()
        {
            if (!IsVisible)
            {
                return;
            }

            ClearPlayersItems();
            CreatePlayersItems();
        }

        private void UpdatePlayerUserItem(int playerActorNumber)
        {
            if (!IsVisible)
            {
                return;
            }

            _playersUserItems[playerActorNumber].UpdateItem();
        }

        private void ClearPlayersItems()
        {
            foreach (Transform childItem in _playersItemsParent)
            {
                Destroy(childItem.gameObject);
                _playersUserItems.Clear();
            }
        }

        private void CreatePlayersItems()
        {
            UserItem userItemType = _userProfile.IsAdmin ? _adminUserItemPrefab : _userItemPrefab;

            _photonRoomPlayers.GetPlayersModels().ForEach(model =>
            {
                UserItem userItem = Instantiate(userItemType, _playersItemsParent);
                userItem.InitializeItem(model);
                _playersUserItems.Add(model.ActorNumber, userItem);
            });
        }
    }
}