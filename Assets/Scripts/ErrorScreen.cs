using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ErrorScreen : MonoBehaviour
{

    public void OkPRessed()
    {
        //ClearObjects.Instance.ClearAll();
        PhotonNetwork.LeaveRoom();
        DataManager.Instance.isTeleportedToTunnel = false;
        DataManager.Instance.isJsonLoaded = false;
        DataManager.Instance.isSelectionDone = false;
        SceneManager.LoadScene(1);
    }
}
