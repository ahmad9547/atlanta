using Metaverse.PresentationModule.Enums;
using System;
using UnityEngine;

namespace Metaverse.PresentationModule.Slides
{
    [Serializable]
    public class ImageSlide : Slide
    {
        private Texture _slideTexture;

        public Texture SlideTexture { get { return _slideTexture; } }

        public ImageSlide()
        {
            this._slideType = SlideType.ImageSlide;
        }

        public void SetTexture(Texture texture)
        {
            _slideTexture = texture;
        }
    }
}
