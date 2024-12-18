using UnityEngine;
using Core.ServiceLocator;

namespace Metaverse.PresentationModule
{
    public sealed class PresentationSlidesProvider : MonoBehaviour
    {
        [SerializeField] private PresentationLoader _presentationLoader;

        #region Services

        private IPresentationSlidesService _presentationSlidesInstance;
        private IPresentationSlidesService _presentationSlides
            => _presentationSlidesInstance ??= Service.Instance.Get<IPresentationSlidesService>();

        #endregion

        private void OnEnable()
        {
            _presentationSlides.AddPhotonEventReceiver();
            _presentationLoader.OnPresentationLoaded.AddListener(OnPresentationDownloaded);
        }

        private void OnDisable()
        {
            _presentationSlides.RemovePhotonEventReceiver();
            _presentationLoader.OnPresentationLoaded.RemoveListener(OnPresentationDownloaded);
        }

        private void OnPresentationDownloaded()
        {
            _presentationSlides.OnPresentationDownloaded(_presentationLoader.LoadedPresentation);
        }
    }
}