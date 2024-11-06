using PhotonEngine.Disconnection;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Common.Clipboard;

namespace StartScene.UI.Controllers
{
    public sealed class NetworkDisconnectionPopup : DisconnectionPopup
    {
        [SerializeField] private TextMeshProUGUI _errorText;

        [SerializeField] private Button _copyErrorTextButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            _copyErrorTextButton.onClick.AddListener(OnCopyErrorClick);
        }

        // leaving empty start to prevent Hide() call from inherited class
        protected override void Start() { }

        protected override void OnDisable()
        {
            base.OnDisable();
            _copyErrorTextButton.onClick.RemoveListener(OnCopyErrorClick);
        }

        protected override void OnDisconnectionInfoUpdated(DisconnectionMessage disconnectionMessage)
        {
            if (disconnectionMessage.DisconnectionType != DisconnectionType.PhotonErrorDisconnection)
            {
                Hide();
                return;
            }

            SetErrorText(disconnectionMessage as PhotonDisconnectionMessage);
            Show();
        }

        private void OnCopyErrorClick()
        {
            ClipboardCopyAPI.CopyTextToClipboard(_errorText.text);
        }

        private void SetErrorText(PhotonDisconnectionMessage disconnectionMessage)
        {
            if (disconnectionMessage != null)
            {
                _errorText.text = disconnectionMessage.Message;
            }
        }
    }
}
