using Core.ServiceLocator;
using Metaverse.Banners.Services;
using UnityEngine;

namespace Metaverse.Banners
{
    public sealed class BannerManager : MonoBehaviour
    {
        #region Services

        private IBannerContentLoaderService _bannerContentLoaderInstance;
        private IBannerContentLoaderService BannerContentLoader
            => _bannerContentLoaderInstance ??= Service.Instance.Get<IBannerContentLoaderService>();

        #endregion

        private void Start()
        {
            BannerContentLoader.LoadBannerTextures();
        }
        
        private void OnDestroy()
        {
            BannerContentLoader.ClearBannerTextures();
        }
    }
}