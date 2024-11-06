using System.Collections.Generic;
using Core.ServiceLocator;
using Metaverse.ErrorHandling.Services;
using UnityEngine;

namespace Metaverse.ErrorHandling
{
    internal sealed class ErrorScreen : MonoBehaviour, IErrorObserver
    {
        [SerializeField] private ErrorPopup _errorPopup;

        private IErrorHandlingService ErrorHandlingService => Service.Instance.Get<IErrorHandlingService>();

        private readonly Queue<Error> _errors = new();

        private void OnEnable() => ErrorHandlingService.AddObserver(this);
        private void OnDisable() => ErrorHandlingService.RemoveObserver(this);

        public void HandleObservation(Error error)
        {
            if (ErrorHandlingService.IsErrorPopupOpen)
            {
                _errors.Enqueue(error);
                _errorPopup.ShowButton();
                return;
            }

            _errorPopup.Show(error, HandleErrorAgreement);
        }

        private void HandleErrorAgreement()
        {
            if (_errors.Count == 0)
            {
                _errorPopup.Close();
                return;
            }
            
            _errorPopup.Hide();
            
            Error nextError = _errors.Dequeue();
            _errorPopup.Show(nextError, HandleErrorAgreement);
        }
    }
}