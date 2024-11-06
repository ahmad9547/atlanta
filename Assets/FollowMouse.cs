using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public float distanceFromSurface = 2;
    public Vector3 offset = Vector3.zero;

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, LayerMask.GetMask("Paintable")))
        {
            transform.position = ((hitInfo.point - (hitInfo.normal * distanceFromSurface)) + offset);
        }
    }
}
