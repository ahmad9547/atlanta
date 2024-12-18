using Core.ServiceLocator;
using Photon.Pun;
using PhotonEngine.Disconnection.Services;
using PhotonEngine.PhotonRoom;
using UnityEngine;
using UserManagement;

namespace PhotonEngine.Disconnection
{
    public sealed class AfkPlayer : MonoBehaviourPunCallbacks
    {
        private const string MouseXInputAxis = "Mouse X";
        private const string MouseYInputAxis = "Mouse Y";
        private const float InGamePlayerAfkTimeInSecondsToKick = 600f;
        private const float BackgroundPlayerAfkTimeInSecondsToKick = 400f;

        #region Services

        private INetworkService _photonRoomInstance;
        private INetworkService _photonRoom
            => _photonRoomInstance ??= Service.Instance.Get<INetworkService>();

        private IDisconnectionCollector _disconnectionCollectorInstance;
        private IDisconnectionCollector _disconnectionCollector
            => _disconnectionCollectorInstance ??= Service.Instance.Get<IDisconnectionCollector>();

        private IUserProfileService _userProfileService;
        private IUserProfileService _userProfile
            => _userProfileService ??= Service.Instance.Get<IUserProfileService>();

        #endregion

        private bool _playerAfkTimerEnabled = false;
        private float _playerAfkTimer = 0f;
        private float _playerAfkTimeInSecondsToKick = InGamePlayerAfkTimeInSecondsToKick;

        private void Update()
        {
            CheckPlayerInput();
            UpdatePlayerAfkTimer();
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            AfkTimerSetActive(!_userProfile.IsAdmin);
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            AfkTimerSetActive(false);
        }

        private void UpdatePlayerAfkTimer()
        {
            if (!_playerAfkTimerEnabled)
            {
                return;
            }

            _playerAfkTimer += Time.deltaTime;

            if (_playerAfkTimer >= _playerAfkTimeInSecondsToKick)
            {
                KickAfkPlayer();
            }
        }

        private void KickAfkPlayer()
        {
            _disconnectionCollector.SetDisconnectionMessage(new AfkDisconnectionMessage());
            _photonRoom.ExitFromNetworkRoom();
        }

        private void CheckPlayerInput()
        {
            if (!_playerAfkTimerEnabled)
            {
                return;
            }

            if (Input.anyKey ||
                Input.GetAxis(MouseXInputAxis) != 0 ||
                Input.GetAxis(MouseYInputAxis) != 0)
            {
                _playerAfkTimer = 0f;
            }
        }

        private void AfkTimerSetActive(bool state)
        {
            _playerAfkTimer = 0f;
            _playerAfkTimerEnabled = state;
        }

        private void OnApplicationFocus(bool focus)
        {
            _playerAfkTimer = 0f;
            _playerAfkTimeInSecondsToKick = focus ? InGamePlayerAfkTimeInSecondsToKick : BackgroundPlayerAfkTimeInSecondsToKick;
        }
    }
}