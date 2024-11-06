using System.Collections.Generic;

namespace ProjectConfig.Banners
{
    public static class BannersConfig
    {
        private static string _temporaryImageUrl;
        private static List<string> _imagesUrl;
        private static List<string> _videosUrl;
        private static List<BannerModel> _banners;

        public static string TemporaryImageUrl => _temporaryImageUrl;
        public static List<string> ImagesUrl => _imagesUrl;
        public static List<string> VideosUrl => _videosUrl;
        public static List<BannerModel> BannersList => _banners;

        public static void SetupBanners(BannersConfigModel bannersConfigModel)
        {
            _temporaryImageUrl = bannersConfigModel.TemporaryImageUrl;
            _imagesUrl = bannersConfigModel.ImagesUrl;
            _videosUrl = bannersConfigModel.VideosUrl;
            _banners = bannersConfigModel.Banners;
        }
    }
}