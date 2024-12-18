using _Project.PlayerUIScene.Scripts.SideMenu.Services;
using UnityEngine;
using Core.UI;
using UnityEngine.UI;
using Core.ServiceLocator;
using Metaverse.PlayersSettings;
using VoiceChat;
using PlayerUIScene.SideMenu.Enums;
using PhotonEngine.CustomProperties;
using PhotonEngine.CustomProperties.Enums;

namespace PlayerUIScene.SideMenu.Controllers
{
    public sealed class MicrophoneStateController : UIController
    {
        [SerializeField] private Button _muteButton;
        [SerializeField] private Button _unmuteButton;
        [SerializeField] private Button _adminMutedButton;

        [SerializeField] private Image _mutedImage;
        [SerializeField] private Image _mutedByAdminImage;

        #region Services

        private IPlayerSettingsService _playerSettingsInstance;
        private IPlayerSettingsService _playerSettings
            => _playerSettingsInstance ??= Service.Instance.Get<IPlayerSettingsService>();

        private IPlayerCustomPropertiesService _playerCustomPropertiesService;
        private IPlayerCustomPropertiesService _playerCustomProperties
            => _playerCustomPropertiesService ??= Service.Instance.Get<IPlayerCustomPropertiesService>();

        private IVoiceChatService _voiceChatService;
        private IVoiceChatService _voiceChat
            => _voiceChatService ??= Service.Instance.Get<IVoiceChatService>();

        private IMicrophoneStateService _microphoneStateInstance;
        private IMicrophoneStateService _microphoneState
            => _microphoneStateInstance ??= Service.Instance.Get<IMicrophoneStateService>();

        #endregion

        private void OnEnable()
        {
            _muteButton.onClick.AddListener(SetMuteState);
            _unmuteButton.onClick.AddListener(SetUnmuteState);
            _microphoneState.SetMutedByAdminEvent.AddListener(SetMutedByAdmin);
            _microphoneState.SetUnmutedByAdminEvent.AddListener(SetUnmutedByAdmin);
        }

        protected override void Start()
        {
            Show();
            SetupMicrophone();
        }

        private void OnDisable()
        {
            _muteButton.onClick.RemoveListener(SetMuteState);
            _unmuteButton.onClick.RemoveListener(SetUnmuteState);
            _microphoneState.SetMutedByAdminEvent.RemoveListener(SetMutedByAdmin);
            _microphoneState.SetUnmutedByAdminEvent.RemoveListener(SetUnmutedByAdmin);
        }

        private void SetMutedByAdmin()
        {
            _voiceChat.MuteInGeneralChannel();
            SetMuteButtonsStates(false, false);
            SetMutedImagesStates(false, true);
            _adminMutedButton.gameObject.SetActive(true);

            _playerCustomProperties.AddOrUpdateLocalPlayerCustomProperty(PlayerCustomPropertyKey.ByAdminMuted);
        }

        private void SetUnmutedByAdmin()
        {
            _adminMutedButton.gameObject.SetActive(false);
            _playerCustomProperties.RemoveLocalPlayerCustomProperty(PlayerCustomPropertyKey.ByAdminMuted);

            switch (_microphoneState.PlayerLastMuteState)
            {
                case MuteState.LocalMuted:
                    {
                        SetMuteState();
                        break;
                    }
                case MuteState.LocalUnmuted:
                    {
                        SetUnmuteState();
                        break;
                    }
            }
        }

        private void SetMuteButtonsStates(bool unmuteButtonState, bool muteButtonState)
        {
            _unmuteButton.gameObject.SetActive(unmuteButtonState);
            _muteButton.gameObject.SetActive(muteButtonState);
        }

        public void SetupMicrophone()
        {
            if (_playerSettings.IsUnmuted)
            {
                SetUnmuteState();
                return;
            }

            SetMuteState();
        }

        private void SetMuteState()
        {
            _microphoneState.SetMuteState(_muteButton);

            SetMuteButtonsStates(true, false);
            SetMutedImagesStates(true, false);
        }

        private void SetUnmuteState()
        {
            _microphoneState.SetUnmuteState(_unmuteButton);

            SetMuteButtonsStates(false, true);
            SetMutedImagesStates(false, false);
        }

        private void SetMutedImagesStates(bool mutedImageState, bool mutedByAdminImageState)
        {
            _mutedImage.gameObject.SetActive(mutedImageState);
            _mutedByAdminImage.gameObject.SetActive(mutedByAdminImageState);
        }
    }
}