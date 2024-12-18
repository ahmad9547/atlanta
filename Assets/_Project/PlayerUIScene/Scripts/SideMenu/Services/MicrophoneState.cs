using Core.ServiceLocator;
using Core.UI;
using Metaverse.PlayersSettings;
using PlayerUIScene.SideMenu.Enums;
using UnityEngine.Events;
using UnityEngine.UI;
using VoiceChat;

namespace _Project.PlayerUIScene.Scripts.SideMenu.Services
{
    public class MicrophoneState : IMicrophoneStateService
    {
        private readonly UIAnimator _uiAnimator = new UIAnimator();

        public UnityEvent SetMutedByAdminEvent { get; } = new UnityEvent();
        public UnityEvent SetUnmutedByAdminEvent { get; } = new UnityEvent();

        public MuteState PlayerLastMuteState { get; private set; }

        #region Services

        private IPlayerSettingsService _playerSettingsInstance;
        private IPlayerSettingsService _playerSettings
            => _playerSettingsInstance ??= Service.Instance.Get<IPlayerSettingsService>();

        private IVoiceChatService _voiceChatService;
        private IVoiceChatService _voiceChat
            => _voiceChatService ??= Service.Instance.Get<IVoiceChatService>();

        #endregion

        public void SetMuteState(Button muteButton)
        {
            _uiAnimator.ButtonScale(muteButton, () =>
            {
                _playerSettings.IsUnmuted = false;
                _voiceChat.MuteInGeneralChannel();
                PlayerLastMuteState = MuteState.LocalMuted;
            });
        }

        public void SetUnmuteState(Button unmuteButton)
        {
            _uiAnimator.ButtonScale(unmuteButton, () =>
            {
                _playerSettings.IsUnmuted = true;
                _voiceChat.UnmuteInGeneralChannel();
                PlayerLastMuteState = MuteState.LocalUnmuted;
            });
        }

        public void SetMutedByAdmin()
        {
            SetMutedByAdminEvent?.Invoke();
        }

        public void SetUnmutedByAdmin()
        {
            SetUnmutedByAdminEvent?.Invoke();
        }
    }
}