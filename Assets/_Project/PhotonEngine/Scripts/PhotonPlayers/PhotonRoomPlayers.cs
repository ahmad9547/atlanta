using System.Collections.Generic;
using System.Linq;
using Core.ServiceLocator;
using Photon.Pun;
using Photon.Realtime;
using PhotonEngine.CustomProperties;
using PhotonEngine.CustomProperties.Enums;
using PhotonEngine.PhotonPlayers.Interfaces;
using UnityEngine;
using UserManagement;

namespace PhotonEngine.PhotonPlayers
{
    public class PhotonRoomPlayers : IPhotonRoomPlayersService
    {
        public List<IPhotonPlayersObserver> PhotonPlayersObservers { get; } = new List<IPhotonPlayersObserver>();
        public List<INewPhotonPlayerObserver> NewPhotonPlayerObservers { get; } = new List<INewPhotonPlayerObserver>();
        public List<IPhotonPlayerLeaveObserver> PhotonPlayerLeaveObservers { get; } = new List<IPhotonPlayerLeaveObserver>();

        #region Services

        private IPlayerCustomPropertiesService _playerCustomPropertiesService;
        private IPlayerCustomPropertiesService _playerCustomProperties
            => _playerCustomPropertiesService ??= Service.Instance.Get<IPlayerCustomPropertiesService>();

        #endregion

        public void AddPlayersObserver(IPhotonPlayersObserver observer)
        {
            if (PhotonPlayersObservers.Contains(observer))
            {
                return;
            }
            PhotonPlayersObservers.Add(observer);
        }

        public void RemovePlayersObserver(IPhotonPlayersObserver observer)
        {
            if (!PhotonPlayersObservers.Contains(observer))
            {
                return;
            }

            PhotonPlayersObservers.Remove(observer);
        }

        public List<PlayersMenuUserModel> GetPlayersModels()
        {
            List<PlayersMenuUserModel> userModels = new List<PlayersMenuUserModel>();
            List<Player> players = GetPlayers();

            try
            {
                foreach (Player player in players)
                {
                    if (player.Equals(PhotonNetwork.LocalPlayer))
                    {
                        continue;
                    }

                    PlayersMenuUserModel userModel = new PlayersMenuUserModel()
                    {
                        Nickname = player.NickName,
                        AvatarId = (string)_playerCustomProperties.GetSpecialPlayerCustomProperty(player, PlayerCustomPropertyKey.AvatarLink),
                        ActorNumber = player.ActorNumber,
                        PhotonPlayer = player
                    };

                    userModels.Add(userModel);
                }
            }
            catch (System.Exception exception)
            {
                Debug.LogError(exception);
            }

            return userModels;
        }

        public int NumberOfPlayers()
        {
            return PhotonNetwork.CurrentRoom.PlayerCount;
        }

        public List<Player> GetPlayers()
        {
            return PhotonNetwork.PlayerList.ToList();
        }

        public void AddNewPlayerObserver(INewPhotonPlayerObserver observer)
        {
            if (NewPhotonPlayerObservers.Contains(observer))
            {
                return;
            }

            NewPhotonPlayerObservers.Add(observer);
        }

        public void RemoveNewPlayerObserver(INewPhotonPlayerObserver observer)
        {
            if (!NewPhotonPlayerObservers.Contains(observer))
            {
                NewPhotonPlayerObservers.Remove(observer);
            }

            NewPhotonPlayerObservers.Remove(observer);
        }

        public void AddPlayerLeaveObserver(IPhotonPlayerLeaveObserver observer)
        {
            if (PhotonPlayerLeaveObservers.Contains(observer))
            {
                return;
            }

            PhotonPlayerLeaveObservers.Add(observer);
        }

        public void RemovePlayerLeaveObserver(IPhotonPlayerLeaveObserver observer)
        {
            if (!PhotonPlayerLeaveObservers.Contains(observer))
            {
                return;
            }

            PhotonPlayerLeaveObservers.Remove(observer);
        }
    }
}