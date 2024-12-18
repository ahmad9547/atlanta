using Core.ServiceLocator;
using Core.UI;
using PhotonEngine.Disconnection;
using PhotonEngine.Disconnection.Services;
using UnityEngine;
using UnityEngine.Events;

namespace StartScene.UI.Controllers
{
    public sealed class DisconnectionScreen : UIController
    {
        [HideInInspector] public UnityEvent<DisconnectionMessage> DisconnectionInfoUpdated;

        #region Services

        private IDisconnectionCollector _disconnectionCollectorInstance;
        private IDisconnectionCollector _disconnectionCollector
            => _disconnectionCollectorInstance ??= Service.Instance.Get<IDisconnectionCollector>();

        #endregion

        protected override void Start()
        {
            base.Start();
            CheckDisconnectionInfo();
        }

        public void OnDisconnectionConfirmed()
        {
            _disconnectionCollector.ResetDisconnectionInfo();
            Hide();
        }

        private void CheckDisconnectionInfo()
        {
            if (!_disconnectionCollector.HasDisconnectionInfo)
            {
                Hide();
                return;
            }

            DisconnectionInfoUpdated?.Invoke(_disconnectionCollector.DisconnectionMessage);
            Show();
        }
    }
}
