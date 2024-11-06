using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TunnelSceneFlowManager : MonoBehaviour
{
    public GameObject errorObject;
    public TextMeshProUGUI errorText;

    public GameObject LoadingPanel;
    public TextMeshProUGUI loadingText;

    public MultiplayerTunnel multiplayerTunnel;

    public InstantiatePlayer instantiatePlayer;

    public Vector3 pos;

    public GameObject SavingObject;
    

    private void Start()
    {
        loadingText.text = "Setting Up tunnel";
        DataManager.Instance.InstantiateTunnel();
    }

    public void TunnelLoadedStartPainting(Vector3 spawnPoint)
    {
        pos = spawnPoint;
        loadingText.text = "Loading user generated graffiti\n0%";
        DataManager.Instance.Start_Call();
    }

    public void UpdateLoadText(int val)
    {
        loadingText.text = $"Downloading user generated graffiti\n{val}%";
    }

    public void UpdatePaintText(int val)
    {
        loadingText.text = $"Painting user generated graffiti\n{val}%";
    } 

    public void TunnelPainted()
    {
        loadingText.text = "Setting Photon Server";
        multiplayerTunnel.StartMultiplayer();
    }

    public void ShowError(string s)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        DataManager.Instance.controller?.ShowCursor();
        errorText.text = s; 
        errorObject?.SetActive(true);
    }

    public void RoomJoinedLoadPLayer()
    {
        loadingText.text = "Setting Player Setup";
        instantiatePlayer.InstantiateToASpecificPostion(pos);
    }

}
