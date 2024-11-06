using System;
using System.Collections;
using System.Collections.Generic;
using Avatars.AvatarLoading;
using Avatars.Enums;
using Avatars.Services;
using Avatars.WebGLMovement;
using Core.ServiceLocator;
using PaintIn3D;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AvatarInit : MonoBehaviourPunCallbacks , IPunInstantiateMagicCallback
{
    public SprayCanMovement sprayCanMovement;
    public SprayCanVisual sprayCanVisual;
    public Animator playerAnimator;
    public Camera camera;
    public Controller controller;

    public AnimatorOverrideController remotePainter;
    public AnimatorOverrideController remoteSpectator;
    public AnimatorOverrideController localSpectator;
    public AnimatorOverrideController localPainter;
    public PlayerModeSwitch playerModeSwitch;
    public LoadTunnel loadTunnel;
    internal GameObject sprayMesh;

    private void Start()
    {
        if (!photonView.IsMine)
        {
           sprayCanMovement.GetComponent<DistanceBasedPainting>().enabled = false;
        }
        else
        {
			controller.SprayCanMovement = sprayCanMovement;
			controller.SprayCanVisual = sprayCanVisual;
			controller.GetComponent<CwHitScreen>().Camera = camera;
		}
    }

    public IEnumerator WaitForInstantiate(PhotonMessageInfo info)
    {
        yield return new WaitUntil(() =>  AvatarLoader.AvatarTransform == null && controller.isAvatarSetupDone==false);
        bool isPainter = (bool)info.photonView.InstantiationData[0];
        if (photonView.IsMine)
        {
            Debug.Log("Init: Photon View is Mine");
            ChangeMode_Local(isPainter);
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                DataManager.Instance?.LoadTunnelCall();
            }
            
        }
        else
        {
            Debug.Log("Init: Photon View is NOT Mine");
            ChangeMode_Remote(isPainter);
        }
        Loading.Instance.loading.SetActive(false);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        StartCoroutine(WaitForInstantiate(info));
	}

	public void ChangeMode_Local(bool isPainter)
    {
		if (isPainter)
		{
			//Debug.Log("Local Painter");
			StartCoroutine(Routine_SetSprayMeshState(true));
			playerModeSwitch.SetToPainter();
			playerAnimator.runtimeAnimatorController = localPainter;
		}
		else
		{
			//Debug.Log("Local Spectator");
			StartCoroutine(Routine_SetSprayMeshState(false));
			playerModeSwitch.SetToSpectator();
			playerAnimator.runtimeAnimatorController = localSpectator;
		}
	}

    public void ChangeMode_Remote(bool isPainter)
    {
		if (isPainter)
		{
			Debug.Log("Remote Painter");
			StartCoroutine(Routine_SetSprayMeshState(true));
			playerAnimator.runtimeAnimatorController = remotePainter;
		    AvatarLoader.AvatarTransform?.SetActive(true);
		}
		else
		{
			Debug.Log("Remote Spectator");
			StartCoroutine(Routine_SetSprayMeshState(false));
			playerAnimator.runtimeAnimatorController = remoteSpectator;
		    AvatarLoader.AvatarTransform?.SetActive(true);
		}
	}

    public void ChangeMode(bool isPainter)
    {
        photonView.RPC(nameof(RPC_ChangeMode), RpcTarget.All, isPainter);
    }
    
    [PunRPC]
    private void RPC_ChangeMode(bool isPainter)
    {
        if (photonView.IsMine)
        {
            Debug.Log("Photon View is Mine");
            ChangeMode_Local(isPainter);
        }
        else
        {
			Debug.Log("Photon View is NOT Mine");
			ChangeMode_Remote(isPainter);
		}
    }

    private IEnumerator Routine_SetSprayMeshState(bool active)
    {
        yield return new WaitUntil(() => sprayMesh != null);
        sprayMesh.SetActive(active);
    }
}