using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class AutoRoomTest : MonoBehaviourPunCallbacks
{
    public GameObject StartGameObject;
    public GameObject loading;
    public bool isTunnelLoaded;
    public List<RoomInfo> roomInfo;

    public GameObject errorScreen;
    public TextMeshProUGUI errorText;
    public LoadTunnel loadTunnel;

    public InstantiatePlayer instantiatePlayer;

    public void Start()
    {
        loading.SetActive(true);
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        this.Invoke(() => { ConnectToServer(); }, 2.5f);
        //this.Invoke(() => { DataManager.Instance.Start_Call(); }, 1.0f);
        
       // ConnectToServer();
    }

    

    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        this.Invoke(() => { CreateOrJoinRoom(); }, 3.0f);
    }

    public void CreateOrJoinRoom()
    {
        string s = "";
        RoomInfo roomData = null;
        if (!isTunnelLoaded)
        {
            s = "Test";
        }
        else
        {
            s = "Tunnel";
        }
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

    public void ShowError(string s)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        DataManager.Instance.controller?.ShowCursor();
        errorText.text = s;
        errorScreen?.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (loading.activeInHierarchy)
        {
            return;
        }
        ShowError("Dissconnected from Photon Server. Press Ok To reload Game");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        ShowError("Not able to join Photon Room. Press ok to reload Game.");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        roomInfo = roomList;
    }

    public override void OnJoinedRoom()
    {
        loading?.SetActive(false);
        //StartCoroutine(Routine_WaitForModeSelection());
    }

    private IEnumerator Routine_WaitForModeSelection()
    {
        yield return new WaitUntil(() => ModeSelection.Instance.isSelectionDone);
        //StartGameObject.SetActive(true);
        
    }

    public void CallInstantiate()
    {
        instantiatePlayer.InstantiatePlayerLocal();
    }
}