using Metaverse.PresentationModule.Slides;
using System.Collections.Generic;
using UnityEngine;
using Core.FilesReading;
using System.IO;
using UnityEngine.Events;
using Core.ServiceLocator;
using ProjectConfig.General;
using ProjectConfig.PresentationSlides;

namespace Metaverse.PresentationModule
{
    public sealed class PresentationLoader : MonoBehaviour
    {
        [HideInInspector] public UnityEvent OnPresentationLoaded;

        public List<Slide> LoadedPresentation { get; } = new List<Slide>();

        #region Services

        private IWebRequestsLoaderService _webRequestsLoaderInstance;
        private IWebRequestsLoaderService _webRequestsLoader
            => _webRequestsLoaderInstance ??= Service.Instance.Get<IWebRequestsLoaderService>();

        #endregion

        private void Start()
        {
            LoadPresentation();
        }

        private async void LoadPresentation()
        {
            LoadedPresentation.Clear();

            foreach (PresentationSlideModel model in PresentationSlidesConfig.PresentationSlides)
            {
                if (model.IsImage)
                {
                    ImageSlide imageSlide = new ImageSlide();
                    Texture imageTexture = await _webRequestsLoader.DownloadTexture(GetContentPath(model.Url));

                    imageSlide.SetTexture(imageTexture);
                    LoadedPresentation.Add(imageSlide);

                    continue;
                }

                VideoSlide videoSlide = new VideoSlide(GetContentPath(model.Url));
                Texture videoPreview = await _webRequestsLoader.DownloadTexture(GetContentPath(model.PreviewUrl));

                videoSlide.SetPreviewTexture(videoPreview);
                LoadedPresentation.Add(videoSlide);
            }

            OnPresentationLoaded?.Invoke();
        }

        private string GetContentPath(string contentName)
        {
            return Path.Combine(GeneralSettings.ContentFolderUrl, contentName);
        }
    }
}
