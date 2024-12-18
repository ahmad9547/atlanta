using Core.ServiceLocator;
using Photon.Pun;
using Photon.Realtime;
using PhotonEngine.Disconnection.Services;

namespace PhotonEngine.Disconnection
{
    public sealed class DisconnectionCollectorProvider : MonoBehaviourPunCallbacks
    {
        #region Services

        private IDisconnectionCollector _disconnectionCollectorInstance;
        private IDisconnectionCollector _disconnectionCollector
            => _disconnectionCollectorInstance ??= Service.Instance.Get<IDisconnectionCollector>();

        #endregion

        public override void OnDisconnected(DisconnectCause cause)
        {
            CheckDisconnectionCause(cause);
        }

        private void CheckDisconnectionCause(DisconnectCause disconnectCause)
        {
            if (disconnectCause == DisconnectCause.DisconnectByClientLogic)
            {
                return;
            }

            _disconnectionCollector.SetDisconnectionMessage(new PhotonDisconnectionMessage(disconnectCause));
        }
    }
}
