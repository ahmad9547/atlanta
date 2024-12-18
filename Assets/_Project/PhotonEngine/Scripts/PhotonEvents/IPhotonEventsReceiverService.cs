using Core.ServiceLocator;
using PhotonEngine.PhotonEvents.Interfaces;

namespace PhotonEngine.PhotonEvents
{
    public interface IPhotonEventsReceiverService : IService
    {
        void AddCallbackTarget();
        void RemoveCallbackTarget();
        void AddPhotonEventReceiver(IPhotonEventReceiver receiver);
        void RemovePhotoEventReceiver(IPhotonEventReceiver receiver);
    }
}