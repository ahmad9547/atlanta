using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Core.FilesReading;
using Core.ServiceLocator;
using ProjectConfig.Banners;
using ProjectConfig.General;
using UnityEngine;
using UnityEngine.Events;

namespace Metaverse.Banners.Services
{
    public sealed class BannerContentLoader : IBannerContentLoaderService
    {
        #region Services

        private IWebRequestsLoaderService _webRequestsLoaderInstance;
        private IWebRequestsLoaderService _webRequestsLoader
            => _webRequestsLoaderInstance ??= Service.Instance.Get<IWebRequestsLoaderService>();

        #endregion

        public Texture TemporaryTexture { get; private set; }

        public List<Texture> BannerTextures { get; } = new List<Texture>();

        public UnityEvent OnTexturesLoaded { get; } = new UnityEvent();

        public void LoadBannerTextures()
        {
            LoadTextures();
        }

        public void ClearBannerTextures()
        {
            BannerTextures.Clear();
        }

        private async void LoadTextures()
        {
            TemporaryTexture =
                await LoadTexture(Path.Combine(GeneralSettings.ContentFolderUrl, BannersConfig.TemporaryImageUrl));

            foreach (string url in BannersConfig.ImagesUrl)
            {
                string bannerImagePath = Path.Combine(GeneralSettings.ContentFolderUrl, url);
                BannerTextures.Add(await LoadTexture(bannerImagePath));
            }

            OnTexturesLoaded?.Invoke();
        }

        private async Task<Texture> LoadTexture(string path)
        {
            return await _webRequestsLoader.DownloadTexture(path);
        }
    }
}