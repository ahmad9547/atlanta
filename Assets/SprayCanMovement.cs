using PaintCore;
using UnityEngine;

public class SprayCanMovement : MonoBehaviour
{
    public float maxDistance= 2;
    public float distanceFromSurface = 2;
    public Vector3 offset = Vector3.zero;
    
    private Vector3 restPosition = Vector3.zero;
    private Vector3 restForward = Vector3.zero;

    public Controller controller;

    private void OnEnable()
    {
        restPosition = transform.localPosition;
        restForward = transform.forward;
    }

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, LayerMask.GetMask("Paintable")))
        {
            transform.position = (hitInfo.point - (hitInfo.normal * distanceFromSurface)) + offset;
            transform.LookAt(hitInfo.point);
        }
        else
        {
            if (controller.IsDrawing())
            {
                controller.Trip();
            }

            ResetPaintCan();
        }
    }

    private void ResetPaintCan()
    {
        transform.localPosition = restPosition;
        transform.forward = restForward;
    }
}