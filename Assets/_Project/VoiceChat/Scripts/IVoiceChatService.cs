using Core.ServiceLocator;
using UnityEngine.Events;

namespace VoiceChat
{
    public interface IVoiceChatService : IService
    {
        void MuteInAdminChannel();
        void UnmuteInAdminChannel();
        void SetAdminVoiceLocalVolume(float volumeLevel);
        void MuteInGeneralChannel();
        void UnmuteInGeneralChannel();
        void SetPlayerVolumeLevel(int playerId, float volumeLevel);
        void SetVoiceChatSoundLevel(float value);
    }
}