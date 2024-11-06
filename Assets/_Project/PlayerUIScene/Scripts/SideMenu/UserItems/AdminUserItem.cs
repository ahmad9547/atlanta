using UnityEngine;
using UnityEngine.UI;
using Core.ServiceLocator;
using PhotonEngine.PhotonEvents;
using PhotonEngine.PhotonEvents.Enums;
using UserManagement;
using PlayerUIScene.SideMenu.Mute;

namespace PlayerUIScene.SideMenu.UserItems
{
    public sealed class AdminUserItem : UserItem
    {
        [Header("Admin UserItem settings")]
        [SerializeField] private Button _muteButton;
        [SerializeField] private Button _unmuteButton;
        [SerializeField] private Button _kickButton;

        #region Services

        private IPhotonEventsSenderService _photonEventsSenderInstance;
        private IPhotonEventsSenderService _photonEventsSender
            => _photonEventsSenderInstance ??= Service.Instance.Get<IPhotonEventsSenderService>();

        private IPersonalAdminMuteService _personalAdminMuteInstance;
        private IPersonalAdminMuteService _personalAdminMute
            => _personalAdminMuteInstance ??= Service.Instance.Get<IPersonalAdminMuteService>();

        private IGlobalAdminMuteService _globalAdminMuteInstance;
        private IGlobalAdminMuteService _globalAdminMute
            => _globalAdminMuteInstance ??= Service.Instance.Get<IGlobalAdminMuteService>();

        #endregion

        protected override void OnEnable()
        {
            base.OnEnable();
            _muteButton.onClick.AddListener(OnMuteButtonClick);
            _unmuteButton.onClick.AddListener(OnUnmuteButtonClick);
            _kickButton.onClick.AddListener(OnKickButtonClick);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _muteButton.onClick.RemoveListener(OnMuteButtonClick);
            _unmuteButton.onClick.RemoveListener(OnUnmuteButtonClick);
            _kickButton.onClick.RemoveListener(OnKickButtonClick);
        }

        public override void InitializeItem(PlayersMenuUserModel playerModel)
        {
            base.InitializeItem(playerModel);
            SetupMicrophoneState();
        }

        public override void UpdateItem()
        {
            SetupMicrophoneState();
        }

        private void SetupMicrophoneState()
        {
            if (_personalAdminMute.IsPlayerByAdminMuted(GetUserItemActorNumber()))
            {
                SetMuteButtonsStates(false, true);
                return;
            }

            if (_globalAdminMute.IsGlobalMuteEnabled())
            {
                SetMuteButtonsStates(false, true);
                return;
            }

            SetMuteButtonsStates(true, false);
        }

        private void OnMuteButtonClick()
        {
            _uiAnimator.ButtonScale(_muteButton, () =>
            {
                SetMuteButtonsStates(false, true);
                _personalAdminMute.SendPersonalAdminMute(GetUserItemActorNumber());
            });
        }

        private void OnUnmuteButtonClick()
        {
            _uiAnimator.ButtonScale(_unmuteButton, () =>
            {
                SetMuteButtonsStates(true, false);
                _personalAdminMute.SendPersonalAdminUnmute(GetUserItemActorNumber());
            });
        }

        private void OnKickButtonClick()
        {
            _uiAnimator.ButtonScale(_kickButton, () =>
            {
                _photonEventsSender.SendPhotonEvent(PhotonEventCode.KickEventCode, GetUserItemActorNumber());
            });
        }

        private void SetMuteButtonsStates(bool muteButtonState, bool unmuteButtonState)
        {
            _muteButton.gameObject.SetActive(muteButtonState);
            _unmuteButton.gameObject.SetActive(unmuteButtonState);
        }

        private int GetUserItemActorNumber()
        {
            return _playerModel.ActorNumber;
        }
    }
}