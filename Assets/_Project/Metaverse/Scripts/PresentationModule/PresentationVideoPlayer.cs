using System.Collections.Generic;
using Metaverse.PresentationModule.Interfaces;
using Metaverse.PresentationModule.Slides;
using Metaverse.UrlVideoPlayerModule;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace Metaverse.PresentationModule
{
    public class PresentationVideoPlayer : IPresentationVideoPlayerService
    {
        private readonly List<IUrlVideoLoaderHandler> _videoLoaderHandlers = new List<IUrlVideoLoaderHandler>();
        private readonly List<IPresentationVideoPlayer> _videoPlayerObservers = new List<IPresentationVideoPlayer>();
        private VideoPlayer _videoPlayer;

        public UnityEvent PlayVideoEvent { get; set; } = new UnityEvent();
        public UnityEvent PauseVideoEvent { get; set; } = new UnityEvent();
        public UnityEvent ResetVideoEvent { get; set; } = new UnityEvent();

        public IPresentationVideoPlayerService.CheckIfPausedHandler CheckIfPausedEvent { get; set; }

        public void Initialize(VideoPlayer videoPlayer)
        {
            _videoPlayer = videoPlayer;
        }

        public void AddUrlVideoLoaderHandler(IUrlVideoLoaderHandler handler)
        {
            if (_videoLoaderHandlers.Contains(handler))
            {
                Debug.Log("This handler was already added");
                return;
            }

            _videoLoaderHandlers.Add(handler);
        }

        public void RemoveUrlVideoLoaderHandler(IUrlVideoLoaderHandler handler)
        {
            if (!_videoLoaderHandlers.Contains(handler))
            {
                Debug.Log("This handler was not added");
                return;
            }

            _videoLoaderHandlers.Remove(handler);
        }

        public void PlayVideo()
        {
            PlayVideoEvent?.Invoke();
            _videoPlayerObservers.ForEach(observer => observer.VideoPlayed());
        }

        public void PauseVideo()
        {
            PauseVideoEvent?.Invoke();
            _videoPlayerObservers.ForEach(observer => observer.VideoPaused());
        }

        public void ResetVideo()
        {
            ResetVideoEvent?.Invoke();
            _videoPlayerObservers.ForEach(observer => observer.VideoReseted());
        }

        public bool CheckIfPaused()
        {
            return CheckIfPausedEvent.Invoke();
        }

        public void AddVideoPlayerObserver(IPresentationVideoPlayer observer)
        {
            if (_videoPlayerObservers.Contains(observer))
            {
                Debug.Log("This observer was already added");
                return;
            }

            _videoPlayerObservers.Add(observer);
        }

        public void RemoveVideoPlayerObserver(IPresentationVideoPlayer observer)
        {
            if (!_videoPlayerObservers.Contains(observer))
            {
                Debug.Log("This observer was not added");
                return;
            }

            _videoPlayerObservers.Remove(observer);
        }

        public void PresentationSlideSelected(UrlVideoLoader urlVideoLoader, Slide slide)
        {
            urlVideoLoader.StopLoader();

            if (slide.SlideType != Enums.SlideType.VideoSlide)
            {
                return;
            }

            VideoSlide videoSlide = slide as VideoSlide;

            if (videoSlide == null)
            {
                return;
            }

            _videoLoaderHandlers.ForEach(handler => handler.OnUrlVideoStartLoading());

            if (_videoPlayer == null)
            {
                Debug.LogError("Video player is not initialized!");
                return;
            }

            urlVideoLoader.PrepareVideoByUrl(videoSlide.VideoUrl, () =>
                {
                    _videoLoaderHandlers.ForEach(handler => handler.OnUrlVideoLoaderPrepared(_videoPlayer.texture));
                });
        }
    }
}