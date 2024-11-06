namespace PhotonEngine
{
    public interface INetworkCallbacks
    {
        void OnConnectedToNetworkLobby();

        void OnJoinedNetworkRoom();

        void OnJoinedNetworkRoomFailed();

        void OnDisconnectedFromNetworkServer();
    }
}