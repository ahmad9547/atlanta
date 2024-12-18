using System;
using System.Collections.Generic;

namespace ProjectConfig.Banners
{
    [Serializable]
    public sealed class BannersConfigModel
    {
        public string TemporaryImageUrl;
        public List<string> ImagesUrl;
        public List<string> VideosUrl;
        public List<BannerModel> Banners;
    }
}