using Core.ServiceLocator;

namespace Metaverse.PresentationModule
{
    public interface IPresentationVideoSyncService : IService
    {
        void AddPhotonEventReceiver();
        void RemovePhotonEventReceiver();
        void SendVideoPlayed();
        void SendVideoPaused();
        void SendVideoReseted();
    }
}