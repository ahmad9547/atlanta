using UnityEngine;
using Core.UI;
using UnityEngine.UI;
using Common.PlayerInput.Interfaces;
using Core.ServiceLocator;
using Metaverse.PresentationModule.Interfaces;
using Metaverse.PresentationModule;
using Metaverse.PresentationModule.Slides;

namespace PlayerUIScene.SpeakerMenu.Controllers
{
    public sealed class SpeakerVideoPlayerController : UIController, ISpeakerZoneObserver,
        IPresentationSlideSelector, IUrlVideoLoaderHandler
    {
        [SerializeField] private Image _playVideoImage;
        [SerializeField] private Image _pauseVideoImage;

        #region Services

        private IPlayerInputEventHandler _playerInputEventHandlerInstance;
        private IPlayerInputEventHandler _playerInputEventHandler
            => _playerInputEventHandlerInstance ??= Service.Instance.Get<IPlayerInputEventHandler>();

        private IPresentationSlidesService _presentationSlidesInstance;
        private IPresentationSlidesService _presentationSlides
            => _presentationSlidesInstance ??= Service.Instance.Get<IPresentationSlidesService>();

        private ISpeakerZoneCheckerService _speakerZoneCheckerService;
        private ISpeakerZoneCheckerService _speakerZoneChecker
            => _speakerZoneCheckerService ??= Service.Instance.Get<ISpeakerZoneCheckerService>();

        private IPresentationVideoSyncService _presentationVideoSyncInstance;
        private IPresentationVideoSyncService _presentationVideoSync
            => _presentationVideoSyncInstance ??= Service.Instance.Get<IPresentationVideoSyncService>();

        private IPresentationVideoPlayerService _presentationVideoPlayerInstance;
        private IPresentationVideoPlayerService _presentationVideoPlayer
            => _presentationVideoPlayerInstance ??= Service.Instance.Get<IPresentationVideoPlayerService>();

        #endregion

        private void OnEnable()
        {
            _speakerZoneChecker.AddSpeakerZoneObserver(this);
            _presentationSlides.AddPresentationSlideObserver(this);
            _presentationVideoPlayer.AddUrlVideoLoaderHandler(this);
        }

        private void OnDisable()
        {
            _speakerZoneChecker.RemoveSpeakerZoneObserver(this);
            _presentationSlides.RemovePresentationSlideObserver(this);
            _presentationVideoPlayer.RemoveUrlVideoLoaderHandler(this);
        }

        public void SpeakerZoneEntered()
        {
            _playerInputEventHandler.OnPlayPausePresentationVideo?.AddListener(OnPresentationVideoPlayPauseClick);
            _playerInputEventHandler.OnPresentationVideoReset?.AddListener(OnPresentationVideoResetClick);
        }

        public void SpeakerZoneLeft()
        {
            _playerInputEventHandler.OnPlayPausePresentationVideo?.RemoveListener(OnPresentationVideoPlayPauseClick);
            _playerInputEventHandler.OnPresentationVideoReset?.RemoveListener(OnPresentationVideoResetClick);
        }

        public void PresentationSlideSelected(Slide slide)
        {
            Hide();
        }

        public void OnUrlVideoStartLoading()
        {
            Hide();
        }

        public void OnUrlVideoLoaderPrepared(Texture videoPlayerTexture)
        {
            SetVideoPlayerImagesStates(true, false);
            Show();
        }

        public void SetVideoPlayerImagesStates(bool playImageState, bool pauseImageState)
        {
            _playVideoImage.gameObject.SetActive(playImageState);
            _pauseVideoImage.gameObject.SetActive(pauseImageState);
        }

        private void OnPresentationVideoPlayPauseClick()
        {
            if (!IsVisible)
            {
                return;
            }

            if (_presentationVideoPlayer.CheckIfPaused())
            {
                SetVideoPlayerImagesStates(false, true);
                _presentationVideoPlayer.PlayVideo();
                _presentationVideoSync.SendVideoPlayed();
                return;
            }

            SetVideoPlayerImagesStates(true, false);
            _presentationVideoPlayer.PauseVideo();
            _presentationVideoSync.SendVideoPaused();
        }

        private void OnPresentationVideoResetClick()
        {
            if (!IsVisible)
            {
                return;
            }

            _presentationVideoPlayer.ResetVideo();
            _presentationVideoSync.SendVideoReseted();
        }
    }
}