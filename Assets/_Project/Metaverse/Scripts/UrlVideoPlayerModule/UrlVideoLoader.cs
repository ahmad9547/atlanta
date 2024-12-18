using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace Metaverse.UrlVideoPlayerModule
{
    public sealed class UrlVideoLoader : MonoBehaviour
    {
        [SerializeField] private VideoPlayer _videoPlayer;

        private Coroutine _prepareVideoCoroutine;

        public VideoPlayer VideoPlayer => _videoPlayer;

        public void PrepareVideoByUrl(string url, Action OnVideoPrepared = null)
        {
            if (_videoPlayer == null)
            {
                Debug.LogError("Can't prepare video, because video player wasn't set");
                return;
            }

            if (_prepareVideoCoroutine != null)
            {
                StopCoroutine(_prepareVideoCoroutine);
            }

            _prepareVideoCoroutine = StartCoroutine(PrepareVideo(url, OnVideoPrepared));
        }

        public void StopLoader()
        {
            if (_prepareVideoCoroutine != null)
            {
                StopCoroutine(_prepareVideoCoroutine);
            }
        }

        private IEnumerator PrepareVideo(string videoUrl, Action OnVideoPrepared)
        {
            _videoPlayer.url = videoUrl;
            _videoPlayer.renderMode = VideoRenderMode.APIOnly;
            _videoPlayer.Prepare();

            while (!_videoPlayer.isPrepared)
            {
                yield return null;
            }

            _videoPlayer.Pause();

            OnVideoPrepared?.Invoke();
        }
    }
}
