using PhotonEngine.PhotonEvents.Enums;

namespace PhotonEngine.PhotonEvents.Interfaces
{
    public interface IPhotonEventReceiver
    {
        void PhotonEventReceived(PhotonEventCode photonEventCode, object content);
    }
}