using Metaverse.PresentationModule.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Metaverse.Banners
{
    public sealed class BannerView : MonoBehaviour, IUrlVideoLoaderHandler
    {
        [SerializeField] private BannerVideoPlayer _bannerVideoPlayer;
        [SerializeField] private RawImage _bannerViewImage;

        private void OnEnable()
        {
            _bannerVideoPlayer.AddUrlVideoLoaderHandler(this);
        }

        private void OnDisable()
        {
            _bannerVideoPlayer.RemoveUrlVideoLoaderHandler(this);
        }

        public void OnUrlVideoStartLoading() {}

        public void OnUrlVideoLoaderPrepared(Texture videoPlayerTexture)
        {
            _bannerViewImage.texture = videoPlayerTexture;
        }
    }
}