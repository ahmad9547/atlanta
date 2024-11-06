using Core.ServiceLocator;

namespace VoiceChat.Player
{
    public interface ISpatialVoiceManagerService : IService
    {
        float GetSpatialVoiceMaxDistance();
        void RegisterSpatialVoicePlayer(int playerId, PlayerSpatialVoice playerSpatialVoice);
        void UnregisterSpatialVoicePlayer(int playerId);
        void UpdateSpatialVoiceCheckTimer();
    }
}