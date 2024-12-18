using Core.ServiceLocator;
using Photon.Realtime;
using PhotonEngine.PhotonEvents;
using PhotonEngine.PhotonEvents.Enums;
using PhotonEngine.PhotonEvents.Interfaces;

namespace Metaverse.PresentationModule
{
    public class PresentationVideoSync : IPresentationVideoSyncService, IPhotonEventReceiver
    {
        #region Services

        private IPhotonEventsReceiverService _photonEventsReceiverInstance;
        private IPhotonEventsReceiverService _photonEventsReceiver
            => _photonEventsReceiverInstance ??= Service.Instance.Get<IPhotonEventsReceiverService>();

        private IPhotonEventsSenderService _photonEventsSenderInstance;
        private IPhotonEventsSenderService _photonEventsSender
            => _photonEventsSenderInstance ??= Service.Instance.Get<IPhotonEventsSenderService>();

        private IPresentationVideoPlayerService _presentationVideoPlayerInstance;
        private IPresentationVideoPlayerService _presentationVideoPlayer
            => _presentationVideoPlayerInstance ??= Service.Instance.Get<IPresentationVideoPlayerService>();

        #endregion

        public void AddPhotonEventReceiver()
        {
            _photonEventsReceiver.AddPhotonEventReceiver(this);
        }

        public void RemovePhotonEventReceiver()
        {
            _photonEventsReceiver.RemovePhotoEventReceiver(this);
        }

        public void SendVideoPlayed()
        {
            _photonEventsSender.SendPhotonEvent(PhotonEventCode.PresentationVideoPlay, ReceiverGroup.Others);
        }

        public void SendVideoPaused()
        {
            _photonEventsSender.SendPhotonEvent(PhotonEventCode.PresentationVideoPause, ReceiverGroup.Others);
        }

        public void SendVideoReseted()
        {
            _photonEventsSender.SendPhotonEvent(PhotonEventCode.PresentationVideoReset, ReceiverGroup.Others);
        }

        public void PhotonEventReceived(PhotonEventCode photonEventCode, object content)
        {
            switch (photonEventCode)
            {
                case PhotonEventCode.PresentationVideoPlay:
                {
                    _presentationVideoPlayer.PlayVideo();
                    break;
                }
                case PhotonEventCode.PresentationVideoPause:
                {
                    _presentationVideoPlayer.PauseVideo();
                    break;
                }
                case PhotonEventCode.PresentationVideoReset:
                {
                    _presentationVideoPlayer.ResetVideo();
                    break;
                }
            }
        }
    }
}