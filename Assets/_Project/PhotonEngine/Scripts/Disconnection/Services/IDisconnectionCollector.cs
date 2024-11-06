using Core.ServiceLocator;

namespace PhotonEngine.Disconnection.Services
{
    public interface IDisconnectionCollector : IService
    {
        bool HasDisconnectionInfo { get; }

        DisconnectionMessage DisconnectionMessage { get; }

        void SetDisconnectionMessage(DisconnectionMessage message);

        void ResetDisconnectionInfo();
    }
}