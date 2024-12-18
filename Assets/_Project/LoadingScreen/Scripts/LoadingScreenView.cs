using UnityEngine;
using TMPro;
using Core.ServiceLocator;

namespace LoadingScreenScene
{
    public sealed class LoadingScreenView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _progressText;


        private ILoadingScreenService _loadingScreenInstance;

        private ILoadingScreenService _loadingScreen
            => _loadingScreenInstance ??= Service.Instance.Get<ILoadingScreenService>();

        private void OnEnable()
        {
            _loadingScreen.OnProgress.AddListener(OnProgress);
        }

        private void OnDisable()
        {
            _loadingScreen.OnProgress.RemoveListener(OnProgress);
        }

        private void OnProgress(string progressMessage)
        {
            _progressText.SetText(progressMessage);
        }
    }
}
