using Core.ServiceLocator;
using Photon.Realtime;

namespace PhotonEngine.PhotonRoom
{
    public interface INetworkService : IService
    {
        void Connect();

        void JoinOrCreateNetworkRoom(string locationName);

        void LeaveCurrentRoom();

        void ExitFromNetworkRoom();

        void AddNetworkRoomCallbacksObserver(INetworkCallbacks observer);

        void RemoveNetworkRoomCallbacksObserver(INetworkCallbacks observer);

        void OnConnectedToMaster();

        void OnJoinedLobby();

        void OnJoinedRoom();

        void OnJoinRoomFailed(short returnCode, string message);

        void OnDisconnected(DisconnectCause cause);
    }
}