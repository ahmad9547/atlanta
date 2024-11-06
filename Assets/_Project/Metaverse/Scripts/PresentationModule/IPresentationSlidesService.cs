using System.Collections.Generic;
using Core.ServiceLocator;
using Metaverse.PresentationModule.Interfaces;
using Metaverse.PresentationModule.Slides;

namespace Metaverse.PresentationModule
{
    public interface IPresentationSlidesService : IService
    {
        void AddPhotonEventReceiver();
        void RemovePhotonEventReceiver();
        void SelectPreviousSlide();
        void SelectNextSlide();
        void AddPresentationSlideObserver(IPresentationSlideSelector observer);
        void RemovePresentationSlideObserver(IPresentationSlideSelector observer);
        void OnPresentationDownloaded(List<Slide> slides);
    }
}