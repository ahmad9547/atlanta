using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace LoadingScreenScene
{
    public sealed class LoadingScreen : ILoadingScreenService
    {
        private const string LoadingScreenScene = "LoadingScreenScene";

        public UnityEvent<string> OnProgress { get; } = new UnityEvent<string>();

        public UnityEvent OnScreenLoaded { get; } = new UnityEvent();

        private Scene _loadingScreenScene;

        private bool _isScreenVisible;
        private bool _isLoading;

        public void Show()
        {
            if (_isScreenVisible)
            {
                OnScreenLoaded?.Invoke();
                return;
            }

            if (_isLoading)
            {
                return;
            }

            _isLoading = true;
            SceneManager.LoadSceneAsync(LoadingScreenScene, LoadSceneMode.Additive)
                .completed += OnLoadingScreenLoaded;
        }

        public void Hide()
        {
            if (!_isScreenVisible)
            {
                return;
            }

            Scene activeScene = SceneManager.GetActiveScene();

            if (activeScene.name.Equals(LoadingScreenScene) ||
                _loadingScreenScene == null)
            {
                Debug.LogError("Error on attempt of unloading LoadingScreen scene");
                return;
            }

            SceneManager.UnloadSceneAsync(_loadingScreenScene).completed
                += OnLoadingScreenUnloaded;
        }

        public void SetLoadingProgressMessage(string message)
        {
            OnProgress?.Invoke(message);
        }

        private void OnLoadingScreenLoaded(AsyncOperation handle)
        {
            _isScreenVisible = true;
            _isLoading = false;

            _loadingScreenScene = SceneManager.GetSceneByName(LoadingScreenScene);

            SceneManager.SetActiveScene(_loadingScreenScene);

            OnScreenLoaded?.Invoke();

            handle.completed -= OnLoadingScreenLoaded;
        }

        private void OnLoadingScreenUnloaded(AsyncOperation handle)
        {
            _isScreenVisible = false;

            handle.completed -= OnLoadingScreenUnloaded;
        }
    }
}
