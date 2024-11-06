using UnityEngine;
using Core.ServiceLocator;

namespace Metaverse.PresentationModule
{
    public sealed class PresentationVideoSyncProvider : MonoBehaviour
    {
        #region Services

        private IPresentationVideoSyncService _presentationVideoSyncInstance;
        private IPresentationVideoSyncService _presentationVideoSync
            => _presentationVideoSyncInstance ??= Service.Instance.Get<IPresentationVideoSyncService>();

        #endregion

        private void OnEnable()
        {
            _presentationVideoSync.AddPhotonEventReceiver();
        }

        private void OnDisable()
        {
            _presentationVideoSync.RemovePhotonEventReceiver();
        }
    }
}