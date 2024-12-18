using Core.ServiceLocator;
using System;
using UnityEngine.Events;

namespace LoadingScreenScene
{
    public interface ILoadingScreenService : IService
    {
        UnityEvent<string> OnProgress { get; }

        UnityEvent OnScreenLoaded { get; }

        void Show();

        void Hide();

        void SetLoadingProgressMessage(string message);
    }
}