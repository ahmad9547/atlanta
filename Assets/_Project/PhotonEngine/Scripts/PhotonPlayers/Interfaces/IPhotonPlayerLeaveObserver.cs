using Photon.Realtime;

namespace PhotonEngine.PhotonPlayers.Interfaces
{
    public interface IPhotonPlayerLeaveObserver
    {
        void PlayerLeftRoom(Player player);
    }
}