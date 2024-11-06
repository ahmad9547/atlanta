using Metaverse.PresentationModule.Enums;
using System;
using UnityEngine;

namespace Metaverse.PresentationModule.Slides
{
    [Serializable]
    public class VideoSlide : Slide
    {
        private Texture _videoPreview;
        private string _videoUrl;

        public Texture VideoPreview => _videoPreview;

        public string VideoUrl => _videoUrl;

        public VideoSlide()
        {
            this._slideType = SlideType.VideoSlide;
        }

        public VideoSlide(string videoUrl)
        {
            _videoUrl = videoUrl;
            _slideType = SlideType.VideoSlide;
        }

        public void SetPreviewTexture(Texture texture)
        {
            _videoPreview = texture;
        }
    }
}
