using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using System.Collections.Generic;
using Metaverse.Analytics;
using ProjectConfig.Session;

namespace PhotonEngine.PhotonRoom
{
    public class Network : INetworkService
    {
        private const int PlayerTimeToLiveAfterDisconnect = 60000;
        private const int RoomTimeToLiveAfterDisconnect = 60000;

        private readonly RoomOptions _roomOptions = new RoomOptions()
        {
            PlayerTtl = PlayerTimeToLiveAfterDisconnect,
            EmptyRoomTtl = RoomTimeToLiveAfterDisconnect,
            CleanupCacheOnLeave = true
        };

        private readonly List<INetworkCallbacks> _roomCallbacksObservers = new List<INetworkCallbacks>();


        public void Connect()
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.AutomaticallySyncScene = false;
        }

        public void OnConnectedToMaster()
        {
            EventLogger.Log(PhotonConnectionEventType.ConnectedToServer,
                new EventParameter<string>("nickName", PhotonNetwork.NickName));

            PhotonNetwork.JoinLobby();
        }

        public void OnJoinedLobby()
        {
            EventLogger.Log(PhotonConnectionEventType.JoinedLobby,
                new EventParameter<string>("nickName", PhotonNetwork.NickName));

            _roomCallbacksObservers.ForEach(observer => observer.OnConnectedToNetworkLobby());
        }

        public void OnJoinedRoom()
        {
            EventLogger.Log(PhotonConnectionEventType.JoinedRoom,
                new EventParameter<string>("nickName", PhotonNetwork.NickName));

            _roomCallbacksObservers.ForEach(observer => observer.OnJoinedNetworkRoom());
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            _roomCallbacksObservers.ForEach(observer => observer.OnJoinedNetworkRoomFailed());
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            EventLogger.Log(PhotonConnectionEventType.DisconnectedFromServer,
                new EventParameter<string>("nickName", PhotonNetwork.NickName),
                new EventParameter<string>("cause", cause.ToString()));

            _roomCallbacksObservers.ForEach(observer => observer.OnDisconnectedFromNetworkServer());
        }

        public void JoinOrCreateNetworkRoom(string locationName)
        {
            PhotonNetwork.JoinOrCreateRoom(SessionConfig.RoomId + locationName, _roomOptions, null);
        }

        public void LeaveCurrentRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void ExitFromNetworkRoom()
        {
            PhotonNetwork.Disconnect();
        }

        public void AddNetworkRoomCallbacksObserver(INetworkCallbacks observer)
        {
            if (_roomCallbacksObservers.Contains(observer))
            {
                Debug.LogError("Observer already added to list of observers");
                return;
            }

            _roomCallbacksObservers.Add(observer);
        }

        public void RemoveNetworkRoomCallbacksObserver(INetworkCallbacks observer)
        {
            if (!_roomCallbacksObservers.Contains(observer))
            {
                Debug.LogError("Observer can not be found in list of observers");
                return;
            }

            _roomCallbacksObservers.Remove(observer);
        }
    }
}