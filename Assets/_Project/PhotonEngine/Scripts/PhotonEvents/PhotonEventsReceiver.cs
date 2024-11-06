using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using PhotonEngine.PhotonEvents.Enums;
using PhotonEngine.PhotonEvents.Interfaces;

namespace PhotonEngine.PhotonEvents
{
    public class PhotonEventsReceiver : IPhotonEventsReceiverService, IOnEventCallback
    {
        private readonly List<IPhotonEventReceiver> _eventReceivers = new List<IPhotonEventReceiver>();

        public void AddCallbackTarget()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public void RemoveCallbackTarget()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void AddPhotonEventReceiver(IPhotonEventReceiver receiver)
        {
            _eventReceivers.Add(receiver);
        }

        public void RemovePhotoEventReceiver(IPhotonEventReceiver receiver)
        {
            _eventReceivers.Remove(receiver);
        }

        public void OnEvent(EventData photonEvent)
        {
            PhotonEventCode eventCode = (PhotonEventCode)photonEvent.Code;
            object eventData = photonEvent.CustomData;
            _eventReceivers.ForEach(receiver => receiver.PhotonEventReceived(eventCode, eventData));
        }
    }
}