using System.Collections.Generic;
using System.Linq;
using Core.ServiceLocator;
using Metaverse.PresentationModule.Interfaces;
using Metaverse.PresentationModule.Slides;
using Photon.Realtime;
using PhotonEngine.PhotonEvents;
using PhotonEngine.PhotonEvents.Enums;
using PhotonEngine.PhotonEvents.Interfaces;
using UnityEngine;

namespace Metaverse.PresentationModule
{
    public class PresentationSlides : IPresentationSlidesService, IPhotonEventReceiver
    {
        private readonly List<IPresentationSlideSelector> _presentationSlidesObservers = new List<IPresentationSlideSelector>();

        private LinkedListNode<KeyValuePair<int, Slide>> _linkedSlidesNode;
        private LinkedList<KeyValuePair<int, Slide>> _linkedSlides;
        private List<Slide> _presentationSlidesList;

        #region Services

        private IPhotonEventsReceiverService _photonEventsReceiverInstance;
        private IPhotonEventsReceiverService _photonEventsReceiver
            => _photonEventsReceiverInstance ??= Service.Instance.Get<IPhotonEventsReceiverService>();

        private IPhotonEventsSenderService _photonEventsSenderInstance;
        private IPhotonEventsSenderService _photonEventsSender
            => _photonEventsSenderInstance ??= Service.Instance.Get<IPhotonEventsSenderService>();

        #endregion

        public void AddPhotonEventReceiver()
        {
            _photonEventsReceiver.AddPhotonEventReceiver(this);
        }

        public void RemovePhotonEventReceiver()
        {
            _photonEventsReceiver.RemovePhotoEventReceiver(this);
        }

        public void SelectPreviousSlide()
        {
            _linkedSlidesNode = _linkedSlidesNode.Previous ?? _linkedSlides.Last;
            SelectSlide(_linkedSlidesNode.Value.Value);

            _photonEventsSender.SendPhotonEvent(PhotonEventCode.PresentationSlideSwitch, ReceiverGroup.Others, _linkedSlidesNode.Value.Key);
        }

        public void SelectNextSlide()
        {
            _linkedSlidesNode = _linkedSlidesNode.Next ?? _linkedSlides.First;
            SelectSlide(_linkedSlidesNode.Value.Value);

            _photonEventsSender.SendPhotonEvent(PhotonEventCode.PresentationSlideSwitch, ReceiverGroup.Others, _linkedSlidesNode.Value.Key);
        }

        public void AddPresentationSlideObserver(IPresentationSlideSelector observer)
        {
            if (_presentationSlidesObservers.Contains(observer))
            {
                Debug.Log("This observer was already added");
                return;
            }

            _presentationSlidesObservers.Add(observer);
        }

        public void RemovePresentationSlideObserver(IPresentationSlideSelector observer)
        {
            if (!_presentationSlidesObservers.Contains(observer))
            {
                Debug.Log("This observer was not added");
                return;
            }

            _presentationSlidesObservers.Remove(observer);
        }

        public void PhotonEventReceived(PhotonEventCode photonEventCode, object content)
        {
            if (photonEventCode != PhotonEventCode.PresentationSlideSwitch)
            {
                return;
            }

            int slideIndex = (int) content;

            if (_presentationSlidesList.Count <= slideIndex
                || _presentationSlidesList[slideIndex] == null)
            {
                return;
            }

            SelectSlide(_presentationSlidesList[slideIndex]);

            _linkedSlidesNode =
                _linkedSlides.Find(new KeyValuePair<int, Slide>(slideIndex, _presentationSlidesList[slideIndex]));
        }

        public void OnPresentationDownloaded(List<Slide> slides)
        {
            _presentationSlidesList = slides;
            SetupSlidesLinkedList();
            SetupFirstSlide();
        }

        private void SelectSlide(Slide slide)
        {
            _presentationSlidesObservers.ForEach(observer => observer.PresentationSlideSelected(slide));
        }

        private void SetupSlidesLinkedList()
        {
            _linkedSlides = new LinkedList<KeyValuePair<int, Slide>>(_presentationSlidesList
                .Select((slide, index) => new KeyValuePair<int, Slide>(index, slide)));
        }

        private void SetupFirstSlide()
        {
            _linkedSlidesNode = _linkedSlides.First;
            SelectSlide(_linkedSlidesNode.Value.Value);
        }
    }
}