using System;

namespace ProjectConfig.Banners
{
    [Serializable]
    public sealed class BannerModel
    {
        public int ImageId;
        public int VideoId;
        public bool IsActive;
        public bool IsImage;
        public bool IsPermanent;
        public BannerResolutionModel BannerResolutionModel;
    }
}