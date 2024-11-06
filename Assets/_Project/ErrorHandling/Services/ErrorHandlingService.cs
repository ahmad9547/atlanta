using System.Collections.Generic;
using Metaverse.Analytics;
using UnityEngine;
using UnityEngine.Events;

namespace Metaverse.ErrorHandling.Services
{
    internal sealed class ErrorHandlingService : IErrorHandlingService
    {
        private readonly HashSet<IErrorObserver> _errorObservers = new();

        private readonly HashSet<LogType> _ignoredLogTypes = new()
        {
            LogType.Log,
            LogType.Warning,
        };

        public bool IsErrorPopupOpen { get; set; }

        public UnityEvent OnErrorPopupClosed { get; } = new UnityEvent();

        public void AddObserver(IErrorObserver observer)
        {
            _errorObservers.Add(observer);

#if AUTOMATIC_ERROR_HANDLING_ENABLED
            if (_errorObservers.Count > 0)
            {
                Application.logMessageReceived += HandleLog;
            }
#endif
        }

        public void RemoveObserver(IErrorObserver observer)
        {
            _errorObservers.Remove(observer);

#if AUTOMATIC_ERROR_HANDLING_ENABLED
            if (_errorObservers.Count == 0)
            {
                Application.logMessageReceived -= HandleLog;
            }
#endif
        }

        public void ForceHandleError(Error error)
        {
            HandleError(error);
        }

        public void CloseErrorPopup()
        {
            OnErrorPopupClosed?.Invoke();
        }

        private void HandleLog(string log, string stacktrace, LogType type)
        {
            if (_ignoredLogTypes.Contains(type))
            {
                return;
            }

            ErrorType errorType = type switch
            {
                LogType.Error => ErrorType.Error,
                LogType.Assert => ErrorType.Assert,
                LogType.Exception => ErrorType.Exception,
                _ => ErrorType.Error,
            };

            Error error = new Error(errorType, log);

            HandleError(error);
        }

        private void HandleError(Error error)
        {
            ErrorEventType eventType = error.Type switch
            {
                ErrorType.Error => ErrorEventType.Error,
                ErrorType.Exception => ErrorEventType.Exception,
                ErrorType.Assert => ErrorEventType.Assert,
                _ => ErrorEventType.Error,
            };

            EventLogger.Log(eventType,
                new EventParameter<string>("message", error.Message));

            foreach (IErrorObserver observer in _errorObservers)
            {
                observer.HandleObservation(error);
            }
        }
    }
}