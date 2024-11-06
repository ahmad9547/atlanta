using Metaverse.PresentationModule.Enums;
using System;

namespace Metaverse.PresentationModule.Slides
{
    [Serializable]
    public class Slide
    {
        protected SlideType _slideType;

        public SlideType SlideType { get { return _slideType; } }
    }
}
