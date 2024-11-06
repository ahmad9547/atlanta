using System.Collections;
using UnityEngine;

public class FlickGesture : MonoBehaviour
{
    public float flickTime;
    public float minFlickDistance;
    public Controller controller;
    [Header("Read Only")]
    public float elapsedTime = 0;
    public float distanceDelta = 0;
    public Vector3 flickStartPosition = Vector3.zero;
    public Vector3 taperStartPosition = Vector3.zero;

    private IEnumerator Start()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                flickStartPosition = transform.position;
            }

            if (Input.GetMouseButton(0))
            {
                elapsedTime += Time.deltaTime;

                distanceDelta = Vector3.Distance(flickStartPosition, transform.position);

                if (controller.IsDrawing())
                {
                    if (distanceDelta > minFlickDistance)
                    {
                        if (elapsedTime < flickTime)
                        {
                            if (taperStartPosition == Vector3.zero)
                            {
                                taperStartPosition = transform.position;
                            }

                            float taperDistanceDelta = Vector3.Distance(taperStartPosition, transform.position);
                            float t = Mathf.Clamp01(taperDistanceDelta / minFlickDistance);
                            float lerpedFlow = GetLerpedFlowValue(t);
                            controller.SetPaintFlow(lerpedFlow);

                            // Debug.Log($"LerpedFlow: {lerpedFlow} : TaperDistance: {taperDistanceDelta} : T: {t}");

                            if (t > 0.9) { controller.StopDrawing(); }
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                controller.SetPaintFlow(controller.defaultFlowValue);
                flickStartPosition = Vector3.zero;
                taperStartPosition = Vector3.zero;
                elapsedTime = 0;
            }

            yield return null;
        }
    }

    private float GetLerpedFlowValue(float t)
    {
        return Mathf.Lerp(controller.defaultFlowValue, controller.minFlowValue, t);
    }
}