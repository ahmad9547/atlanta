using Core.UI;
using PhotonEngine.Disconnection;
using UnityEngine;
using UnityEngine.UI;

namespace StartScene.UI.Controllers
{
    public abstract class DisconnectionPopup : UIController
    {
        [SerializeField] protected DisconnectionScreen _disconnectionScreen;

        [SerializeField] protected Button _confirmButton;

        protected virtual void OnEnable()
        {
            _disconnectionScreen.DisconnectionInfoUpdated.AddListener(OnDisconnectionInfoUpdated);
            _confirmButton.onClick.AddListener(OnConfirmButtonClick);
        }

        protected virtual void OnDisable()
        {
            _disconnectionScreen.DisconnectionInfoUpdated.RemoveListener(OnDisconnectionInfoUpdated);
            _confirmButton.onClick.RemoveListener(OnConfirmButtonClick);
        }

        protected abstract void OnDisconnectionInfoUpdated(DisconnectionMessage disconnectionMessage);

        private void OnConfirmButtonClick()
        {
            _uiAnimator.ButtonScale(_confirmButton, () =>
            {
                _disconnectionScreen.OnDisconnectionConfirmed();
                Hide();
            });
        }
    }
}
