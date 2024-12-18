using _Project.PlayerUIScene.Scripts.SideMenu.Services;
using Core.ServiceLocator;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UserManagement;
using PhotonEngine.CustomProperties.Enums;
using PhotonEngine.PhotonEvents;
using PhotonEngine.PhotonEvents.Interfaces;
using PhotonEngine.PhotonEvents.Enums;

namespace PlayerUIScene.SideMenu.Mute
{
    public sealed class GlobalAdminMuteProvider : MonoBehaviour, IPhotonEventReceiver
    {
        [SerializeField] private GameObject _globalMuteContent;
        [SerializeField] private Button _muteAllButton;
        [SerializeField] private Button _unmuteAllButton;

        #region Services

        private IUserProfileService _userProfileService;
        private IUserProfileService _userProfile
            => _userProfileService ??= Service.Instance.Get<IUserProfileService>();

        private IMicrophoneStateService _microphoneStateInstance;
        private IMicrophoneStateService _microphoneState
            => _microphoneStateInstance ??= Service.Instance.Get<IMicrophoneStateService>();

        private IPhotonEventsReceiverService _photonEventsReceiverInstance;
        private IPhotonEventsReceiverService _photonEventsReceiver
            => _photonEventsReceiverInstance ??= Service.Instance.Get<IPhotonEventsReceiverService>();

        private IGlobalAdminMuteService _globalAdminMuteInstance;
        private IGlobalAdminMuteService _globalAdminMute
            => _globalAdminMuteInstance ??= Service.Instance.Get<IGlobalAdminMuteService>();

        #endregion

        private void OnEnable()
        {
            _muteAllButton.onClick.AddListener(OnMuteAllButtonClick);
            _unmuteAllButton.onClick.AddListener(OnUnmuteAllButtonClick);

            _photonEventsReceiver.AddPhotonEventReceiver(this);
        }

        private void Start()
        {
            SetupVisibility();
            CheckGlobalMuteRoomProperty();
        }

        private void OnDisable()
        {
            _muteAllButton.onClick.RemoveListener(OnMuteAllButtonClick);
            _unmuteAllButton.onClick.RemoveListener(OnUnmuteAllButtonClick);

            _photonEventsReceiver.RemovePhotoEventReceiver(this);
        }

        public void PhotonEventReceived(PhotonEventCode photonEventCode, object content)
        {
            switch (photonEventCode)
            {
                case PhotonEventCode.GlobalMuteEventCode:
                {
                    GlobalMute();
                    break;
                }
                case PhotonEventCode.GlobalUnmuteEventCode:
                {
                    GlobalUnmute();
                    break;
                }
            }
        }

        private void OnMuteAllButtonClick()
        {
            SetGlobalMute();
            _globalAdminMute.OnMuteAllButtonClick();
        }

        private void OnUnmuteAllButtonClick()
        {
            SetGlobalUnmute();
            _globalAdminMute.OnUnmuteAllButtonClick();
        }

        private void SetGlobalMute()
        {
            _globalAdminMute.SetGlobalMute();
            SetMuteButtonsStates(false, true);
        }

        private void SetGlobalUnmute()
        {
            _globalAdminMute.SetGlobalUnmute();
            SetMuteButtonsStates(true, false);
        }

        private void GlobalMute()
        {
            SetGlobalMute();
            _microphoneState.SetMutedByAdmin();
        }

        private void GlobalUnmute()
        {
            SetGlobalUnmute();
            _microphoneState.SetUnmutedByAdmin();
        }

        private void SetupVisibility()
        {
            _globalMuteContent.SetActive(_userProfile.IsAdmin);
        }

        private void CheckGlobalMuteRoomProperty()
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(RoomCustomPropertyKey.AllAreMuted.ToString()))
            {
                GlobalMute();
            }
        }

        private void SetMuteButtonsStates(bool muteAllButtonState, bool unmuteAllButtonState)
        {
            _muteAllButton.gameObject.SetActive(muteAllButtonState);
            _unmuteAllButton.gameObject.SetActive(unmuteAllButtonState);
        }
    }
}