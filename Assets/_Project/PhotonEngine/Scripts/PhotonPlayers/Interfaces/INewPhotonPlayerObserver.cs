using Photon.Realtime;

namespace PhotonEngine.PhotonPlayers.Interfaces
{
    public interface INewPhotonPlayerObserver
    {
        void NewPlayerEntered(Player newPlayer);
    }
}