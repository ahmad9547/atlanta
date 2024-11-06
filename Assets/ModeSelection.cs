using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSelection : MonoBehaviour
{
    public static ModeSelection Instance;

    public bool isPainter = false;
    public bool isSelectionDone = false;
    public AutoRoomTest autoRoomTest;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnClick_Painter()
    {
        DataManager.Instance.isPainter = true;
        DataManager.Instance.isSelectionDone = true;
        isPainter = true;
        isSelectionDone = true;
        gameObject.SetActive(false);
		Loading.Instance.loading.SetActive(true);
        autoRoomTest.CallInstantiate();
	}
    
    public void OnClick_Spectator()
    {
        DataManager.Instance.isPainter = false;
        DataManager.Instance.isSelectionDone = true;
        isPainter = false;
        isSelectionDone = true;
        gameObject.SetActive(false);
		Loading.Instance.loading.SetActive(true);
        autoRoomTest.CallInstantiate();
	}
}