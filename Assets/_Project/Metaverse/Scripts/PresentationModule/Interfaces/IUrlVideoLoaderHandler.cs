using UnityEngine;

namespace Metaverse.PresentationModule.Interfaces
{
    public interface IUrlVideoLoaderHandler
    {
        void OnUrlVideoStartLoading();

        void OnUrlVideoLoaderPrepared(Texture videoPlayerTexture);
    }
}