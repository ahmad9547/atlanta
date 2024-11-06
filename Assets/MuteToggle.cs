using _Project.PlayerUIScene.Scripts.SideMenu.Services;
using Core.ServiceLocator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteToggle : MonoBehaviour
{
    public Sprite muteSprite;
    public Sprite unmuteSprite;

    private IMicrophoneStateService _microphoneStateInstance;
    private IMicrophoneStateService _microphoneState
        => _microphoneStateInstance ??= Service.Instance.Get<IMicrophoneStateService>();

    private Button muteButton;

    private void Start()
    {
        muteButton = GetComponent<Button>();
        SetMuteState();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        if (muteButton.image.sprite == muteSprite)
        {
            SetUnmuteState();
        }
        else
        {
            SetMuteState();
        }
    }

    private void SetMuteState()
    {
        _microphoneState.SetMuteState(muteButton);
        muteButton.image.sprite = muteSprite;
    }

    private void SetUnmuteState()
    {
        _microphoneState.SetUnmuteState(muteButton);
        muteButton.image.sprite = unmuteSprite;
    }
}