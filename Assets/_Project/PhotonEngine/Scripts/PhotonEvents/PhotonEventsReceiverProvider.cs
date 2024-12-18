using UnityEngine;
using Core.ServiceLocator;

namespace PhotonEngine.PhotonEvents
{
    public sealed class PhotonEventsReceiverProvider : MonoBehaviour
    {
        #region Services

        private IPhotonEventsReceiverService _photonEventsReceiverInstance;
        private IPhotonEventsReceiverService _photonEventsReceiver
            => _photonEventsReceiverInstance ??= Service.Instance.Get<IPhotonEventsReceiverService>();

        #endregion

        private void OnEnable()
        {
            _photonEventsReceiver.AddCallbackTarget();
        }

        private void OnDisable()
        {
            _photonEventsReceiver.RemoveCallbackTarget();
        }
    }
}