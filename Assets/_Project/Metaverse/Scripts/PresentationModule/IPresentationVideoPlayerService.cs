using Core.ServiceLocator;
using Metaverse.PresentationModule.Interfaces;
using Metaverse.PresentationModule.Slides;
using Metaverse.UrlVideoPlayerModule;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace Metaverse.PresentationModule
{
    public interface IPresentationVideoPlayerService : IService
    {
        UnityEvent PlayVideoEvent { get; }
        UnityEvent PauseVideoEvent { get; }
        UnityEvent ResetVideoEvent { get; }

        delegate bool CheckIfPausedHandler();
        CheckIfPausedHandler CheckIfPausedEvent { get; set; }

        void Initialize(VideoPlayer videoPlayer);
        void AddUrlVideoLoaderHandler(IUrlVideoLoaderHandler handler);
        void RemoveUrlVideoLoaderHandler(IUrlVideoLoaderHandler handler);
        void PlayVideo();
        void PauseVideo();
        void ResetVideo();
        bool CheckIfPaused();
        void AddVideoPlayerObserver(IPresentationVideoPlayer observer);
        void RemoveVideoPlayerObserver(IPresentationVideoPlayer observer);
        void PresentationSlideSelected(UrlVideoLoader urlVideoLoader, Slide slide);
    }
}