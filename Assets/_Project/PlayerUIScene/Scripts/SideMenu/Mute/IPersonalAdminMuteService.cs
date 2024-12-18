using Core.ServiceLocator;
using Photon.Realtime;
using PhotonEngine.PhotonEvents.Enums;

namespace PlayerUIScene.SideMenu.Mute
{
    public interface IPersonalAdminMuteService : IService
    {
        void AddPhotonEventReceiver();
        void RemovePhotoEventReceiver();
        void SendPersonalAdminMute(int actorNumber);
        void SendPersonalAdminUnmute(int actorNumber);
        bool IsPlayerByAdminMuted(int actorNumber);
        void SetPersonalMuteForEachPlayer();
        void SetPersonalUnmuteForEachPlayer();
        void CheckPlayersMuteProperty();
        void PhotonEventReceived(PhotonEventCode photonEventCode, object content);
        void OnPlayerLeftRoom(Player otherPlayer);
    }
}