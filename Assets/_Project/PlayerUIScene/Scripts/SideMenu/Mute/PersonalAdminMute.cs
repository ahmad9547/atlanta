using System;
using System.Collections.Generic;
using System.Linq;
using _Project.PlayerUIScene.Scripts.SideMenu.Services;
using Core.ServiceLocator;
using Photon.Pun;
using Photon.Realtime;
using PhotonEngine.CustomProperties.Enums;
using PhotonEngine.PhotonEvents;
using PhotonEngine.PhotonEvents.Enums;
using PhotonEngine.PhotonEvents.Interfaces;
using PhotonEngine.PhotonPlayers;
using UnityEngine;

namespace PlayerUIScene.SideMenu.Mute
{
    public class PersonalAdminMute : IPersonalAdminMuteService, IPhotonEventReceiver
    {
        private readonly HashSet<int> _personalMutedActorNumbers = new HashSet<int>();

        #region Services

        private IPhotonEventsReceiverService _photonEventsReceiverInstance;
        private IPhotonEventsReceiverService _photonEventsReceiver
            => _photonEventsReceiverInstance ??= Service.Instance.Get<IPhotonEventsReceiverService>();

        private IPhotonEventsSenderService _photonEventsSenderInstance;
        private IPhotonEventsSenderService _photonEventsSender
            => _photonEventsSenderInstance ??= Service.Instance.Get<IPhotonEventsSenderService>();

        private IPhotonRoomPlayersService _photonRoomPlayersInstance;
        private IPhotonRoomPlayersService _photonRoomPlayers
            => _photonRoomPlayersInstance ??= Service.Instance.Get<IPhotonRoomPlayersService>();

        private IMicrophoneStateService _microphoneStateInstance;
        private IMicrophoneStateService _microphoneState
            => _microphoneStateInstance ??= Service.Instance.Get<IMicrophoneStateService>();

        private IUserListService _userListInstance;
        private IUserListService _userList
            => _userListInstance ??= Service.Instance.Get<IUserListService>();

        #endregion

        public void AddPhotonEventReceiver()
        {
            _photonEventsReceiver.AddPhotonEventReceiver(this);
        }

        public void RemovePhotoEventReceiver()
        {
            _photonEventsReceiver.RemovePhotoEventReceiver(this);
        }

        public void SendPersonalAdminMute(int actorNumber)
        {
            if (_personalMutedActorNumbers.Contains(actorNumber))
            {
                return;
            }

            _personalMutedActorNumbers.Add(actorNumber);

            _photonEventsSender.SendPhotonEvent(PhotonEventCode.PersonalMuteEventCode, ReceiverGroup.Others, actorNumber);
        }

        public void SendPersonalAdminUnmute(int actorNumber)
        {
            if (!_personalMutedActorNumbers.Contains(actorNumber))
            {
                return;
            }

            _personalMutedActorNumbers.Remove(actorNumber);

            _photonEventsSender.SendPhotonEvent(PhotonEventCode.PersonalUnmuteEventCode, ReceiverGroup.Others, actorNumber);
        }

        public bool IsPlayerByAdminMuted(int actorNumber)
        {
            return _personalMutedActorNumbers.Contains(actorNumber);
        }

        public void SetPersonalMuteForEachPlayer()
        {
            _photonRoomPlayers.GetPlayers().ForEach(player =>
            {
                _personalMutedActorNumbers.Add(player.ActorNumber);
            });
        }

        public void SetPersonalUnmuteForEachPlayer()
        {
            _photonRoomPlayers.GetPlayers().ForEach(player =>
            {
                _personalMutedActorNumbers.Clear();
            });
        }

        public void CheckPlayersMuteProperty()
        {
            foreach (Player player in _photonRoomPlayers.GetPlayers()
                .Where(player => !player.IsLocal && player.CustomProperties
                    .ContainsKey(PlayerCustomPropertyKey.ByAdminMuted.ToString())))
            {
                SetupPlayerMute(player.ActorNumber);
            }
        }

        public void PhotonEventReceived(PhotonEventCode photonEventCode, object content)
        {
            switch (photonEventCode)
            {
                case PhotonEventCode.PersonalMuteEventCode:
                {
                    int actorNumber = Convert.ToInt32(content);
                    SetupPlayerMute(actorNumber);
                    break;
                }
                case PhotonEventCode.PersonalUnmuteEventCode:
                {
                    int actorNumber = Convert.ToInt32(content);
                    SetupPlayerUnmute(actorNumber);
                    break;
                }
            }
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (!_personalMutedActorNumbers.Contains(otherPlayer.ActorNumber))
            {
                return;
            }

            _personalMutedActorNumbers.Remove(otherPlayer.ActorNumber);
        }

        private void SetupPlayerMute(int actorNumber)
        {
            if (_personalMutedActorNumbers.Contains(actorNumber))
            {
                Debug.LogWarning($"Player with actor number {actorNumber} is already in muted list");
                return;
            }

            _personalMutedActorNumbers.Add(actorNumber);

            if (actorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                _microphoneState.SetMutedByAdmin();
                return;
            }

            _userList.UpdatePlayerUserItem(actorNumber);
        }

        private void SetupPlayerUnmute(int actorNumber)
        {
            if (!_personalMutedActorNumbers.Contains(actorNumber))
            {
                Debug.LogWarning($"There is no player in muted list with actor number {actorNumber}");
                return;
            }

            _personalMutedActorNumbers.Remove(actorNumber);

            if (actorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                _microphoneState.SetUnmutedByAdmin();
                return;
            }

            _userList.UpdatePlayerUserItem(actorNumber);
        }
    }
}