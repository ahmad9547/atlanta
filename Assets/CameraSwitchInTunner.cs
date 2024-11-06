using Avatars.AvatarLoading;
using Avatars.WebGLMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchInTunner : MonoBehaviour
{
    public static bool IsInTunnel = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var switcher = GameObject.FindObjectOfType<CameraSwitcher>();
            if(switcher.isThirdPerson)
            {
                switcher.OnCameraSwitchInput();
                this.Invoke(nameof(EnableFirstPersonObject), 1.0f);
            }

            IsInTunnel = true;
        }
    }

    public void EnableFirstPersonObject()
    {
            var switcher = GameObject.FindObjectOfType<CameraSwitcher>();
        switcher.firstPersonObject.SetActive(!switcher.isThirdPerson);
        if(!switcher.isThirdPerson )
        {
            AvatarLoader.AvatarTransform.SetActive(false);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var switcher = GameObject.FindObjectOfType<CameraSwitcher>();
            if (!switcher.isThirdPerson)
            {
                switcher.OnCameraSwitchInput();
                EnableFirstPersonObject();
        AvatarLoader.AvatarTransform.SetActive(true);
            }

            IsInTunnel = false;
        }
    }
}
