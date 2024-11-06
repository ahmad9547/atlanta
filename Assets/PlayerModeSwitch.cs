using System;
using System.Collections;
using System.Collections.Generic;
using Avatars.AvatarLoading;
using Avatars.Enums;
using Avatars.Services;
using Avatars.WebGLMovement;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerModeSwitch : MonoBehaviourPunCallbacks
{
    public CameraSwitcher cameraSwitcher;

    private void Start()
    {
        PhotonNetwork.AddCallbackTarget(this);            
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            GetComponent<AvatarInit>().ChangeMode(cameraSwitcher.isThirdPerson);
        }
    }

    private object changeModeLock = null;

	internal void SetToPainter()
	{
        if (changeModeLock != null) { return; }
        changeModeLock = new object();

        if (cameraSwitcher.isThirdPerson)
        {
			cameraSwitcher.OnCameraSwitchInput();
			cameraSwitcher.firstPersonObject.SetActive(true);
			AvatarLoader.AvatarTransform.SetActive(false);
		}

		changeModeLock = null;
	}

	internal void SetToSpectator()
	{
		if (changeModeLock != null) { return; }
		changeModeLock = new object();

		if (!cameraSwitcher.isThirdPerson)
        {
			cameraSwitcher.OnCameraSwitchInput();
			cameraSwitcher.firstPersonObject.SetActive(false);
		}

		AvatarLoader.AvatarTransform.SetActive(true);

		changeModeLock = null;
	}
}