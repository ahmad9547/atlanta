using Core.ServiceLocator;
using Photon.Pun;
using Photon.Realtime;

namespace PhotonEngine.PhotonRoom
{
    public sealed class NetworkCallbacksProvider : MonoBehaviourPunCallbacks
    {
        private INetworkService _networkInstance;

        private INetworkService _network
            => _networkInstance ??= Service.Instance.Get<INetworkService>();

        public override void OnConnectedToMaster()
        {
            _network.OnConnectedToMaster();
        }

        public override void OnJoinedLobby()
        {
            _network.OnJoinedLobby();
        }

        public override void OnJoinedRoom()
        {
            _network.OnJoinedRoom();            
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            _network.OnJoinRoomFailed(returnCode, message);            
        }        

        public override void OnDisconnected(DisconnectCause cause)
        {
            _network.OnDisconnected(cause);
        }
    }
}
