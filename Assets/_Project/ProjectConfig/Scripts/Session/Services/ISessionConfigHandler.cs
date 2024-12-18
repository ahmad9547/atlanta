using Core.ServiceLocator;
using UnityEngine.Events;

namespace ProjectConfig.Session.Services
{
    public interface ISessionConfigHandler : IService
    {
        UnityEvent OnSessionConfigInitialized { get; }

        void Initialize();
        void AddErrorMessage(string errorMessage);
        void LogErrors();
    }
}