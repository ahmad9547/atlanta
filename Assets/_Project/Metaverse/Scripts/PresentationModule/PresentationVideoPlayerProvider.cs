using UnityEngine;
using UnityEngine.Video;
using Metaverse.UrlVideoPlayerModule;
using Metaverse.PresentationModule.Interfaces;
using Metaverse.PresentationModule.Slides;
using Core.ServiceLocator;

namespace Metaverse.PresentationModule
{
    public sealed class PresentationVideoPlayerProvider : MonoBehaviour, IPresentationSlideSelector
    {
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private UrlVideoLoader _urlVideoLoader;

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

            _presentationVideoPlayer.PlayVideoEvent.AddListener(PlayVideo);
            _presentationVideoPlayer.PauseVideoEvent.AddListener(PauseVideo);
            _presentationVideoPlayer.ResetVideoEvent.AddListener(ResetVideo);
            _presentationVideoPlayer.CheckIfPausedEvent += CheckIfPaused;
        }

        private void Start()
        {
            _presentationVideoPlayer.Initialize(_videoPlayer);
        }

        private void OnDisable()
        {
            _presentationSlides.RemovePresentationSlideObserver(this);

            _presentationVideoPlayer.PlayVideoEvent.RemoveListener(PlayVideo);
            _presentationVideoPlayer.PauseVideoEvent.RemoveListener(PauseVideo);
            _presentationVideoPlayer.ResetVideoEvent.RemoveListener(ResetVideo);
            _presentationVideoPlayer.CheckIfPausedEvent -= CheckIfPaused;
        }

        public void PresentationSlideSelected(Slide slide)
        {
            StopVideo();
            _presentationVideoPlayer.PresentationSlideSelected(_urlVideoLoader, slide);
        }

        private void PlayVideo()
        {
            _videoPlayer.Play();
        }

        private void PauseVideo()
        {
            _videoPlayer.Pause();
        }

        private void ResetVideo()
        {
            _videoPlayer.time = 0f;
        }

        private void StopVideo()
        {
            _videoPlayer.Stop();
        }

        private bool CheckIfPaused()
        {
            return _videoPlayer.isPaused;
        }
    }
}