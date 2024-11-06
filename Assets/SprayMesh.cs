using System;
using System.Collections;
using System.Collections.Generic;
using Avatars.Enums;
using Photon.Pun;
using UnityEngine;

public class SprayMesh : MonoBehaviour
{
    public GameObject sprayMesh;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);
        
        List<AvatarInit> list = new();
        transform.GetNestedComponentsInParents(list);
        AvatarInit avatarInit = list[0];
        avatarInit.sprayMesh = sprayMesh;
    }
}