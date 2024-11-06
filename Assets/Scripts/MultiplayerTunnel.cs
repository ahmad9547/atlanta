using Metaverse.ErrorHandling;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerTunnel : MonoBehaviourPunCallbacks
{
    private List<RoomInfo> roomInfo;
    public bool isDone;
    public bool isRoomJoined;
    public TunnelSceneFlowManager tunnelSceneFlowManager;
    public bool startDisconnect;
    private void Start()
    {
        startDisconnect = true;
        PhotonNetwork.AddCallbackTarget(this);
        PhotonNetwork.Disconnect();
    }

    public void StartMultiplayer()
    {
        startDisconnect = false;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        while (!PhotonNetwork.IsConnectedAndReady)
        {

        }
        if (isDone)
        {
            return;
        }
        PhotonNetwork.JoinLobby();
    }

    bool isFirstCall = false;

    public override void OnJoinedLobby()
    {
        isFirstCall = false;
    }

    public void CreateOrJoinRoom()
    {
        string s = "";
        RoomInfo roomData = null;
         s = "Tunnel";
        if (roomInfo != null)
        {
            foreach (RoomInfo item in roomInfo)
            {
                if (item.Name.Contains(s))
                {
                    roomData = item;
                    break;
                }
            }
        }
        if (roomData != null)
        {
            PhotonNetwork.JoinRoom(s);
        }
        else
        {
            PhotonNetwork.CreateRoom(s);
        }
    }

    

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        string s = "Tunnel";
        if (message.Contains("A game with the specified id already exist."))
        {
            PhotonNetwork.JoinRoom(s);
            return;
        }
        CreateOrJoinRoom();
        //string s = "Tunnel";
        //PhotonNetwork.JoinRoom(s);
        //TunnelSceneFlowManager.Instance.ShowError("Not Able To Create Room. Press Ok To reload Game");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (!isRoomJoined && !startDisconnect)
        {
            PhotonNetwork.ConnectUsingSettings();
            return;
        }
        if (tunnelSceneFlowManager.SavingObject.activeInHierarchy || startDisconnect)
        {
            return;
        }
        tunnelSceneFlowManager.ShowError("Dissconnected from Photon Server. Press Ok To reload Game");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        CreateOrJoinRoom();
        //TunnelSceneFlowManager.Instance.ShowError("Not able to join Photon Room. Press ok to reload Game.");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        roomInfo = roomList;
        if (!isFirstCall)
        {
            isFirstCall = true;
            this.Invoke(() => { CreateOrJoinRoom(); }, 2.0f);
        }
    }

    public override void OnJoinedRoom()
    {
        isRoomJoined = true;
        tunnelSceneFlowManager.RoomJoinedLoadPLayer();
    }
}
