using Photon.Pun;
using Photon.Realtime;
using PhotonEngine.CustomProperties;
using PhotonEngine.PhotonEvents;
using PhotonEngine.PhotonEvents.Enums;
using PhotonEngine.PhotonEvents.Interfaces;
using PhotonEngine.CustomProperties.Enums;
using UnityEngine;
using System;
using System.Linq;
using Core.ServiceLocator;

namespace Metaverse.SeatingModule
{
    public sealed class MetaverseSeatingPlacesSynch : MonoBehaviourPunCallbacks, IPhotonEventReceiver
    {
        [SerializeField] private MetaverseSeatingPlaces _metaverseSeatingPlaces;

        #region Services

        private IPlayerCustomPropertiesService _playerCustomPropertiesService;
        private IPlayerCustomPropertiesService _playerCustomProperties
            => _playerCustomPropertiesService ??= Service.Instance.Get<IPlayerCustomPropertiesService>();

        private IPhotonEventsReceiverService _photonEventsReceiverInstance;
        private IPhotonEventsReceiverService _photonEventsReceiver
            => _photonEventsReceiverInstance ??= Service.Instance.Get<IPhotonEventsReceiverService>();

        private IPhotonEventsSenderService _photonEventsSenderInstance;
        private IPhotonEventsSenderService _photonEventsSender
            => _photonEventsSenderInstance ??= Service.Instance.Get<IPhotonEventsSenderService>();

        #endregion

        public override void OnEnable()
        {
            base.OnEnable();
            _photonEventsReceiver.AddPhotonEventReceiver(this);
        }

        private void Start()
        {
            CheckPlayersSeatingPropertie();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            _photonEventsReceiver.RemovePhotoEventReceiver(this);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (_playerCustomProperties.IsPlayerHaveCustomProperty(otherPlayer, PlayerCustomPropertyKey.SeatingPlaceId, out object value))
            {
                _metaverseSeatingPlaces.SetSeatingPlaceFreeState(Convert.ToInt32(value), true);
            }
        }

        public void PhotonEventReceived(PhotonEventCode photonEventCode, object content)
        {
            switch (photonEventCode)
            {
                case PhotonEventCode.SeatPlaceOccupiedEventCode:
                    {
                        _metaverseSeatingPlaces.SetSeatingPlaceFreeState((int)content, false);
                        break;
                    }
                case PhotonEventCode.SeatPlaceFreeEventCode:
                    {
                        _metaverseSeatingPlaces.SetSeatingPlaceFreeState((int)content, true);
                        break;
                    }
            }
        }

        public void SendOccupiedSeatingPlaceId(int id)
        {
            _playerCustomProperties.AddOrUpdateLocalPlayerCustomProperty(PlayerCustomPropertyKey.SeatingPlaceId, id);
            _photonEventsSender.SendPhotonEvent(PhotonEventCode.SeatPlaceOccupiedEventCode, ReceiverGroup.Others, id);
        }

        public void SendFreeSeatingPlaceId(int id)
        {
            _playerCustomProperties.RemoveLocalPlayerCustomProperty(PlayerCustomPropertyKey.SeatingPlaceId);
            _photonEventsSender.SendPhotonEvent(PhotonEventCode.SeatPlaceFreeEventCode, ReceiverGroup.Others, id);
        }

        private void CheckPlayersSeatingPropertie()
        {
            foreach (Player player in PhotonNetwork.PlayerList
                .Where(entry => !(entry.IsInactive || entry.IsLocal)))
            {
                if (_playerCustomProperties.IsPlayerHaveCustomProperty(player, PlayerCustomPropertyKey.SeatingPlaceId, out object value))
                {
                    _metaverseSeatingPlaces.SetSeatingPlaceFreeState(Convert.ToInt32(value), false);
                }
            }
        }
    }
}