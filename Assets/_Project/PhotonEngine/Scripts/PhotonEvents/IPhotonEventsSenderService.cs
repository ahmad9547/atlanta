using Core.ServiceLocator;
using Photon.Realtime;
using PhotonEngine.PhotonEvents.Enums;

namespace PhotonEngine.PhotonEvents
{
    public interface IPhotonEventsSenderService : IService
    {
        void SendPhotonEvent(PhotonEventCode code, int[] targetActorsID, object content = null);
        void SendPhotonEvent(PhotonEventCode code, int targetActorID, object content = null);
        void SendPhotonEvent(PhotonEventCode code, ReceiverGroup receiverGroup, object content = null);
    }
}