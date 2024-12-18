using System.Collections.Generic;

namespace ProjectConfig.PresentationSlides
{
    public static class PresentationSlidesConfig
    {
        private static List<PresentationSlideModel> _presentationSlides;

        public static List<PresentationSlideModel> PresentationSlides => _presentationSlides;

        public static void SetupPresentationSlides(PresentationSlidesModel presentationSlidesModel)
        {
            _presentationSlides = presentationSlidesModel.PresentationSlides;
        }
    }
}