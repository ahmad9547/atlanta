using Avatars.Services;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using PhotonEngine.PhotonPlayers;
using PhotonEngine.PhotonEvents.Interfaces;
using PhotonEngine.PhotonEvents;
using PhotonEngine.PhotonEvents.Enums;
using PhotonEngine.PhotonRoom;
using Core.ServiceLocator;
using Metaverse.Services;
using ProjectConfig.Session.Services;

namespace Metaverse
{
    public sealed class MetaverseScene : MonoBehaviourPunCallbacks, IPhotonEventReceiver
    {
        private const string PlayerUISceneName = "PlayerUI";

        #region Services

        private INetworkService _photonRoomInstance;
        private INetworkService _photonRoom
            => _photonRoomInstance ??= Service.Instance.Get<INetworkService>();

        private ILocalPlayerService _localPlayerHolderInstance;
        private ILocalPlayerService _localPlayerHolder
            => _localPlayerHolderInstance ??= Service.Instance.Get<ILocalPlayerService>();

        private IPlayerCreatorService _playerCreatorInstance;
        private IPlayerCreatorService _playerCreator
            => _playerCreatorInstance ??= Service.Instance.Get<IPlayerCreatorService>();

        private IPhotonEventsReceiverService _photonEventsReceiverInstance;
        private IPhotonEventsReceiverService _photonEventsReceiver
            => _photonEventsReceiverInstance ??= Service.Instance.Get<IPhotonEventsReceiverService>();

        private IAvatarLoadingStatusHolderService _avatarLoadingStatusHolderInstance;
        private IAvatarLoadingStatusHolderService _avatarLoadingStatusHolder
            => _avatarLoadingStatusHolderInstance ??= Service.Instance.Get<IAvatarLoadingStatusHolderService>();

        private static ISessionConfigHandler _sessionConfigHandlerService;
        private static ISessionConfigHandler _sessionConfigHandler
            => _sessionConfigHandlerService ??= Service.Instance.Get<ISessionConfigHandler>();

        #endregion

        public override void OnEnable()
        {
            base.OnEnable();
            _photonEventsReceiver.AddPhotonEventReceiver(this);
            _avatarLoadingStatusHolder.OnAvatarLoaded.AddListener(OnAvatarLoaded);
        }

        private void Start()
        {
            SpawnPlayer();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            _photonEventsReceiver.RemovePhotoEventReceiver(this);
            _avatarLoadingStatusHolder.OnAvatarLoaded.RemoveListener(OnAvatarLoaded);
        }

        public void PhotonEventReceived(PhotonEventCode photonEventCode, object content)
        {
            if (!photonEventCode.Equals(PhotonEventCode.KickEventCode))
            {
                return;
            }

            _photonRoom.ExitFromNetworkRoom();
        }

        public override void OnLeftRoom()
        {
            Destroy(this.gameObject);
            SceneManager.UnloadSceneAsync(PlayerUISceneName);
        }

        private void SpawnPlayer()
        {
            GameObject player = _playerCreator.CreatePlayer();

            PhotonView photonView = player.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                _localPlayerHolder.LocalPlayer = player;
            }
        }

        private void OnAvatarLoaded()
        {
            _sessionConfigHandler.LogErrors();
            SceneManager.LoadSceneAsync(PlayerUISceneName, LoadSceneMode.Additive);
        }
    }
}