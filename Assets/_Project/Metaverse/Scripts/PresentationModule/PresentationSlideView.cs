using UnityEngine;
using UnityEngine.UI;
using Core.ServiceLocator;
using Metaverse.PresentationModule.Interfaces;
using Metaverse.PresentationModule.Slides;

namespace Metaverse.PresentationModule
{
    public sealed class PresentationSlideView : MonoBehaviour, IPresentationSlideSelector, IUrlVideoLoaderHandler, IPresentationVideoPlayer
    {
        [SerializeField] private RawImage _slideViewImage;
        [SerializeField] private Image _loadingVideoImage;
        [SerializeField] private RawImage _previewVideoImage;
        [SerializeField] private Image _pauseVideoImage;

        #region Services

        private IPresentationSlidesService _presentationSlidesInstance;
        private IPresentationSlidesService _presentationSlides
            => _presentationSlidesInstance ??= Service.Instance.Get<IPresentationSlidesService>();

        private IPresentationVideoPlayerService _presentationVideoPlayerInstance;
        private IPresentationVideoPlayerService _presentationVideoPlayer
            => _presentationVideoPlayerInstance ??= Service.Instance.Get<IPresentationVideoPlayerService>();

        #endregion

        private void OnEnable()
        {
            _presentationSlides.AddPresentationSlideObserver(this);
            _presentationVideoPlayer.AddUrlVideoLoaderHandler(this);
            _presentationVideoPlayer.AddVideoPlayerObserver(this);
        }

        private void OnDisable()
        {
            _presentationSlides.RemovePresentationSlideObserver(this);
            _presentationVideoPlayer.RemoveUrlVideoLoaderHandler(this);
            _presentationVideoPlayer.RemoveVideoPlayerObserver(this);
        }

        public void PresentationSlideSelected(Slide slide)
        {
            switch (slide.SlideType)
            {
                case Enums.SlideType.ImageSlide:
                    {
                        SetupImageSlide(slide);
                        break;
                    }
                case Enums.SlideType.VideoSlide:
                    {
                        SetupVideoSlide(slide);
                        break;
                    }
            }
        }

        public void OnUrlVideoStartLoading()
        {
            SetVideoImagesStatus(true, true, false);
        }

        public void OnUrlVideoLoaderPrepared(Texture videoPlayerTexture)
        {
            _slideViewImage.texture = videoPlayerTexture;

            SetVideoImagesStatus(true, false, true);
        }

        public void VideoPlayed()
        {
            SetVideoImagesStatus(false, false, false);
        }

        public void VideoPaused()
        {
            SetVideoImagesStatus(false, false, true);
        }

        public void VideoReseted()
        {
        }

        private void SetupImageSlide(Slide slide)
        {
            SetVideoImagesStatus(false, false, false);

            ImageSlide imageSlide = slide as ImageSlide;

            if (imageSlide == null)
            {
                Debug.LogError("Error while casting Slide type to ImageSlide");
                return;
            }

            _slideViewImage.texture = imageSlide.SlideTexture;
        }

        private void SetupVideoSlide(Slide slide)
        {
            VideoSlide videoSlide = slide as VideoSlide;

            if (videoSlide == null)
            {
                Debug.LogError("Error while casting Slide type to VideoSlide");
                return;
            }

            _previewVideoImage.texture = videoSlide.VideoPreview;
        }

        private void SetVideoImagesStatus(bool previewImageState, bool lodingImageState, bool pauseImageState)
        {
            _previewVideoImage.gameObject.SetActive(previewImageState);
            _pauseVideoImage.gameObject.SetActive(pauseImageState);
            _loadingVideoImage.gameObject.SetActive(lodingImageState);
        }
    }
}