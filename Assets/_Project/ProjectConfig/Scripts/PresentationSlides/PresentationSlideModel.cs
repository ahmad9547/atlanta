using System;

namespace ProjectConfig.PresentationSlides
{
    [Serializable]
    public sealed class PresentationSlideModel
    {
        public bool IsImage = true;
        public string Url;
        public string PreviewUrl;
    }
}
