using System.Collections.Generic;
using System.IO;
using Metaverse.PresentationModule.Interfaces;
using Metaverse.UrlVideoPlayerModule;
using ProjectConfig.General;
using UnityEngine;
using UnityEngine.Video;

namespace Metaverse.Banners
{
    public sealed class BannerVideoPlayer : MonoBehaviour
    {
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private UrlVideoLoader _urlVideoLoader;

        private readonly List<IUrlVideoLoaderHandler> _videoLoaderHandlers = new List<IUrlVideoLoaderHandler>();

        public void PrepareVideo(string videoUrl)
        {
            _videoLoaderHandlers.ForEach(handler => handler.OnUrlVideoStartLoading());

            _urlVideoLoader.PrepareVideoByUrl(Path.Combine(GeneralSettings.ContentFolderUrl, videoUrl), () =>
            {
                _videoLoaderHandlers.ForEach(handler => handler.OnUrlVideoLoaderPrepared(_videoPlayer.texture));
                _urlVideoLoader.VideoPlayer.Play();
            });
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
    }
}