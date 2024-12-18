using System;
using System.Collections;
using Avatars.WebGLMovement.MouseControll.Interfaces;
using Common.Clipboard;
using Core.ServiceLocator;
using Metaverse.ErrorHandling.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Metaverse.ErrorHandling
{
    internal sealed class ErrorPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _errorText;
        [SerializeField] private TextMeshProUGUI _redirectionText;
        [SerializeField] private int _redirectTime;

        [Header("Buttons")] [SerializeField] private Button _okButton;
        [SerializeField] private Button _copyButton;

        private IMouseCursorService MouseCursorService => Service.Instance.Get<IMouseCursorService>();
        private IErrorHandlingService ErrorHandlingService => Service.Instance.Get<IErrorHandlingService>();

        private Error _openedError;
        private Action _agreeCallback;
        private bool _wasCursorLocked;
        private Coroutine _redirectCoroutine;

        public void Show(Error error, Action agreeCallback = null)
        {
            _errorText.text = error.Message;

            _agreeCallback = agreeCallback;
            _okButton.onClick.AddListener(HandleErrorAgreement);
            _copyButton.onClick.AddListener(CopyError);

            _wasCursorLocked = MouseCursorService.IsLocked;
            MouseCursorService.UnlockCursor();

            ErrorHandlingService.IsErrorPopupOpen = true;
            _openedError = error;

            gameObject.SetActive(true);
            _okButton.gameObject.SetActive(_openedError?.Url == null);

            if (_openedError?.Url is not null)
            {
                _redirectCoroutine = StartCoroutine(RedirectRoutine());
            }
        }

        public void Hide()
        {
            _okButton.onClick.RemoveListener(HandleErrorAgreement);
            _copyButton.onClick.RemoveListener(CopyError);

            if (_wasCursorLocked)
            {
                MouseCursorService.LockCursor();
            }

            ErrorHandlingService.IsErrorPopupOpen = false;
            _openedError = null;

            if (_redirectCoroutine != null)
            {
                StopCoroutine(_redirectCoroutine);
            }

            gameObject.SetActive(false);
        }

        public void Close()
        {
            if (_openedError.Url != null)
            {
                return;
            }

            ErrorHandlingService.CloseErrorPopup();
            Hide();
        }

        public void ShowButton()
        {
            _okButton.gameObject.SetActive(true);
        }

        private void HandleErrorAgreement() => _agreeCallback?.Invoke();
        private void CopyError() => ClipboardCopyAPI.CopyTextToClipboard(_errorText.text);

        private IEnumerator RedirectRoutine()
        {
            int secondsLeft = _redirectTime;

            while (secondsLeft-- > 0)
            {
                _redirectionText.text = $"Redirection int {secondsLeft} seconds";
                yield return new WaitForSeconds(1);
            }

            if (!string.IsNullOrWhiteSpace(_openedError.Url))
            {
                Application.OpenURL(_openedError.Url);
                _redirectionText.text = "Redirection completed";
                yield break;
            }

            _redirectionText.text = "Failed to redirect";
        }
    }
}