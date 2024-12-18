using Core.ServiceLocator;
using PhotonEngine.CustomProperties;
using PhotonEngine.CustomProperties.Enums;
using VoiceChat.WebGL;

namespace VoiceChat
{
    public class VoiceChat : IVoiceChatService
    {
        private const float AdminChannelLocalVolumeMultiplier = 1000f;
        private const float AdminChatSoundLevelMultiplier = 100f;

        private float _generalVoiceChatSoundLevelCoefficient = 1f;

        #region Services

        private IWebVoiceChatAPIService _webVoiceChatAPIService;
        private IWebVoiceChatAPIService _webVoiceChatAPI
            => _webVoiceChatAPIService ??= Service.Instance.Get<IWebVoiceChatAPIService>();

        private IPlayerCustomPropertiesService _playerCustomPropertiesService;
        private IPlayerCustomPropertiesService _playerCustomProperties
            => _playerCustomPropertiesService ??= Service.Instance.Get<IPlayerCustomPropertiesService>();

        #endregion

        public void MuteInAdminChannel()
        {
            _webVoiceChatAPI.MuteInAdminChannel();
        }

        public void UnmuteInAdminChannel()
        {
            _webVoiceChatAPI.UnmuteInAdminChannel();
        }

        public void SetAdminVoiceLocalVolume(float volumeLevel)
        {
            int volume = (int)(volumeLevel * AdminChannelLocalVolumeMultiplier);
            _webVoiceChatAPI.SetAdminChannelLocalVolume(volume);
        }

        public void MuteInGeneralChannel()
        {
            _webVoiceChatAPI.MuteInGeneralChannel();

            _playerCustomProperties.AddOrUpdateLocalPlayerCustomProperty(PlayerCustomPropertyKey.MicrophoneIsActive, false);
        }

        public void UnmuteInGeneralChannel()
        {
            _webVoiceChatAPI.UnmuteInGeneralChannel();

            _playerCustomProperties.AddOrUpdateLocalPlayerCustomProperty(PlayerCustomPropertyKey.MicrophoneIsActive, true);
        }

        public void SetPlayerVolumeLevel(int playerId, float volumeLevel)
        {
            int volume = (int)(volumeLevel * _generalVoiceChatSoundLevelCoefficient);
            _webVoiceChatAPI.SetRemoteUserVoiceVolume(playerId, volume);
        }

        public void SetVoiceChatSoundLevel(float value)
        {
            _generalVoiceChatSoundLevelCoefficient = value;
            int adminChatVolume = (int)(value * AdminChatSoundLevelMultiplier);
            _webVoiceChatAPI.SetAdminVoiceChatVolumeForAllUsers(adminChatVolume);
        }
    }
}