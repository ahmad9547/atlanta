using System.Collections.Generic;
using Metaverse.ErrorHandling;
using Metaverse.ErrorHandling.Services;
using UnityEngine.Events;

namespace ProjectConfig.Session.Services
{
    public sealed class SessionConfigHandler : ISessionConfigHandler
    {
        private readonly List<string> _errorMessages = new();
        public UnityEvent OnSessionConfigInitialized { get; } = new();

        public void Initialize()
        {
            OnSessionConfigInitialized?.Invoke();
        }

        public void AddErrorMessage(string errorMessage)
        {
            _errorMessages.Add(errorMessage);
        }

        public void LogErrors()
        {
            foreach (string message in _errorMessages)
            {
                ErrorLogger.LogError(ErrorType.Error, message);
            }

            _errorMessages.Clear();
        }
    }
}