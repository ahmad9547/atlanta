using Core.ServiceLocator;
using UnityEngine.Events;

namespace VoiceChat.WebGL
{
    public interface IWebVoiceChatAPIService : IService
    {
        bool IsVoiceChatUsageAllowed { get; set; }
        UnityEvent OnJoinedGeneralVoiceChat { get; }
        UnityEvent OnJoinedAdminVoiceChat { get; }
        UnityEvent<PlayersVolume> OnPlayerVolumesReceived { get; }

        void LeaveChannel();
        void LeaveAdminChannel();
        void MuteInGeneralChannel();
        void UnmuteInGeneralChannel();
        void MuteInAdminChannel();
        void UnmuteInAdminChannel();
        void SetAdminChannelLocalVolume(int volume);
        void SetAdminVoiceChatVolumeForAllUsers(int volume);
        void SetRemoteUserVoiceVolume(int userId, int volume);
        void JoinChannel(int playerUid, string channelName);
        void JoinAdminChannel(int playerUid, string channelName);
        void EnablePlayersVolumeIndicator();
        void JoinGeneralVoiceChat();
        void JoinAdminVoiceChat();
        void ReceivePlayerVolumes(PlayersVolume playersVolume);
    }
}