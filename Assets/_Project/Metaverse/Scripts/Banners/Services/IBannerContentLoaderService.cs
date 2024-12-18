using System.Collections.Generic;
using Core.ServiceLocator;
using UnityEngine;
using UnityEngine.Events;

namespace Metaverse.Banners.Services
{
    public interface IBannerContentLoaderService : IService
    {
        public Texture TemporaryTexture { get; }

        public List<Texture> BannerTextures { get; }

        public UnityEvent OnTexturesLoaded { get; }

        public void LoadBannerTextures();

        public void ClearBannerTextures();
    }
}