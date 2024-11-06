using Core.ServiceLocator;
using UnityEngine.Events;

namespace Metaverse.ErrorHandling.Services
{
    internal interface IErrorHandlingService : IService
    {
        bool IsErrorPopupOpen { get; set; }
        UnityEvent OnErrorPopupClosed { get; }

        void AddObserver(IErrorObserver observer);
        void RemoveObserver(IErrorObserver observer);

        void ForceHandleError(Error error);
        void CloseErrorPopup();
    }
}