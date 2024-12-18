using Core.ServiceLocator;
using UnityEngine;

namespace VoiceChat.Player
{
    public sealed class SpatialVoiceUpdater : MonoBehaviour
    {
        #region Services

        private ISpatialVoiceManagerService _spatialVoiceManagerInstance;
        private ISpatialVoiceManagerService _spatialVoiceManager
            => _spatialVoiceManagerInstance ??= Service.Instance.Get<ISpatialVoiceManagerService>();

        #endregion

        private void Update()
        {
            _spatialVoiceManager.UpdateSpatialVoiceCheckTimer();
        }
    }
}