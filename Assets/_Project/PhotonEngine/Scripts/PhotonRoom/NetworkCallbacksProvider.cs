using Core.ServiceLocator;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

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
            Debug.Log("Player Disconnected");
            if (CanRecoverFromDisconnect(cause))
            {
                Debug.Log("Trying to Recover...");
                Recover(cause);
            }
            else
            {
                _network.OnDisconnected(cause);
            }
        }

        private bool CanRecoverFromDisconnect(DisconnectCause cause)
        {
            switch (cause)
            {
                case DisconnectCause.Exception:
                case DisconnectCause.ServerTimeout:
                case DisconnectCause.ClientTimeout:
                case DisconnectCause.DisconnectByServerLogic:
                case DisconnectCause.DisconnectByServerReasonUnknown:
                    return true;
            }
            return false;
        }

        private void Recover(DisconnectCause cause)
        {
            const int maxAttempts = 5;
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                attempts++;

                if (PhotonNetwork.ReconnectAndRejoin())
                {
                    Debug.Log("ReconnectAndRejoin successful.");
                    return; 
                }
                else if (PhotonNetwork.Reconnect())
                {
                    Debug.Log("Reconnect successful.");
                    return; 
                }
                else if (PhotonNetwork.ConnectUsingSettings())
                {
                    Debug.Log("ConnectUsingSettings successful.");
                    return;
                }

                Debug.LogErrorFormat("Recovery attempt {0}/{1} failed.", attempts, maxAttempts);
            }

            Debug.LogError("All recovery attempts failed. Triggering disconnection handler.");
            _network.OnDisconnected(cause);
        }

    }
}
