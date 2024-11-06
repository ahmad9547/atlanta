using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNoClip : MonoBehaviour
{
    [SerializeField] private float DistanceFromCamera;
    [SerializeField] private float ObjectRadius;

    [SerializeField] private LayerMask LayerMask;

    [SerializeField] private AnimationCurve AnimationCurve;

    private Vector3 _originalLocalPosition;

    private void Start()
    {
        _originalLocalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.SphereCast(Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0f)), ObjectRadius, out var hit, DistanceFromCamera, LayerMask))
        {
            transform.localPosition = _originalLocalPosition -
                                      new Vector3(0.0f, 0.0f,
                                          AnimationCurve.Evaluate(hit.distance / DistanceFromCamera));
        }
        else
        {
            transform.localPosition = _originalLocalPosition;
        }
    }
}
