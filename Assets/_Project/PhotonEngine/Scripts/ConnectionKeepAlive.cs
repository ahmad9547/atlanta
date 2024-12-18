using UnityEngine;
using Photon.Pun;

public class ConnectionKeepAlive : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Method to keep the connection alive
    public void KeepConnectionAlive()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            // Keep the connection alive by sending acknowledgements only
            PhotonNetwork.NetworkingClient.LoadBalancingPeer.SendAcksOnly();
        }
    }
}
