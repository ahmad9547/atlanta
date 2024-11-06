using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISetup : MonoBehaviour
{
    public Controller controller;

    [Header("Key Code Data")]
    public KeyCode colorKey;
    public KeyCode instructionKey;
    public KeyCode closeKey;

    [Header("Color Data")]
    public GameObject colorButton;
    public GameObject colorTab;
    public FlexibleColorPicker colorPicker;

    [Header("Instruction Data")]
    public GameObject instructionPanel;
    public GameObject instructionIcon;
    public GameObject muteIcon;

    [Header("Help Panle Rostrum")]
    public GameObject helpPanelRostrum;

    [Header("Close Panel")]
    public GameObject ClosePanel;
    public GameObject loadingPanel;
    public GameObject closeIcon;
    public GameObject savingData;

    public void Start()
    {
        EventManager.Instance.SetControllerEvent += SetController;
    }

    public void Update()
    {
        if (Input.GetKeyDown(colorKey))
        {
            SetColorPanel();
        }
        if (Input.GetKeyDown(instructionKey))
        {
            SetHelpPanel();
        }
        if (Input.GetKeyDown(closeKey))
        {
            CloseKeyPressed();
        }
    }

    public void CloseKeyPressed()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (loadingPanel.activeInHierarchy)
            {
                return;
            }
            closeIcon.SetActive(false);
            SetIcons(false);
            DisableAll();
            controller.ShowCursor();
            ClosePanel.SetActive(true);
        }
    }

    public void DisableAll()
    {
        helpPanelRostrum.SetActive(false);
        instructionPanel.SetActive(false);
        colorTab.SetActive(false);
        ClosePanel.SetActive(false);
    }

    public void YesPressed()
    {
        savingData.SetActive(true);
        DataManager.Instance.SaveDataPlayerLeft(() => {
            ClearObjects.Instance.ClearAll();
            PhotonNetwork.LeaveRoom();
            DataManager.Instance.isTeleportedToTunnel = false;
            DataManager.Instance.isJsonLoaded = false;
            DataManager.Instance.isSelectionDone = false;
            SceneManager.LoadScene(1);
        });
    }

    public void NoPressed()
    {
        closeIcon.SetActive(true);
        SetIcons(true);
        controller.HideCursor();
        ClosePanel.SetActive(false);
    }

    private void SetIcons(bool x)
    {
        instructionIcon.SetActive(x);
        colorButton.SetActive(x);
        muteIcon.SetActive(x);
    }

    public void SetColorPanel()
    {
        helpPanelRostrum.SetActive(false);
        instructionPanel.SetActive(false);
        bool x = colorTab.activeInHierarchy;
        SetIcons(x);
        controller?.SetColor(colorPicker.color);
        if (x)
        {
            colorTab.SetActive(false);
            controller.HideCursor();
        }
        else
        {
            controller.ShowCursor();
            colorTab.SetActive(true);
        }
    }

    public void SetHelpPanel()
    {
        colorTab.SetActive(false);
        helpPanelRostrum.SetActive(false);
        bool x = instructionPanel.activeInHierarchy;
        SetIcons(x);
        if (x)
        {
            instructionPanel.SetActive(false);
            controller.HideCursor();
        }
        else
        {
            controller.ShowCursor();
            instructionPanel.SetActive(true);
        }
    }

    public void SetRostrumPanel()
    {
        colorTab.SetActive(false);
        instructionPanel.SetActive(false);
        bool x = helpPanelRostrum.activeInHierarchy;
        SetIcons(x);
        if (x)
        {
            helpPanelRostrum.SetActive(false);
            controller.HideCursor();
        }
        else
        {
            controller.ShowCursor();
            helpPanelRostrum.SetActive(true);
        }
    }

    private void SetController(Controller _controller)
    {
        controller = _controller;
    }

    private void OnDisable()
    {
        EventManager.Instance.SetControllerEvent -= SetController;
    }
}
