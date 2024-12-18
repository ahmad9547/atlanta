using Metaverse.UrlVideoPlayerModule;
using UnityEngine;
using UnityEngine.Video;

namespace Metaverse
{
    public sealed class TVVideoPlayer : MonoBehaviour
    {
        [SerializeField] private UrlVideoLoader _urlVideoLoader;
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private Material _tvMaterial;
        
        public void StartVideo(string url)
        {
            _urlVideoLoader.PrepareVideoByUrl(url, () =>
            {
                _tvMaterial.mainTexture = _videoPlayer.texture;
                _videoPlayer.Play();
            });
        }
    }
}
