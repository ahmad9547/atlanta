using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DistanceBasedPainting : MonoBehaviour
{
    public float maxDistance = 2;

    [Header("Read Only")]
    public float distanceToSurface = -1;
    public float flowPerDistance;

    private float distanceTextCountdown = 2f;
    private float distanceTextCountdownValue = 0f;

    public Controller controller;

	private void Start()
    {
        EventManager.Instance.SetControllerEvent += SetController;
        //flowPerDistance = Controller.Instance.defaultFlowValue;
    }

    private void OnDisable()
    {
        EventManager.Instance.SetControllerEvent -= SetController;
    }

    public void SetController(Controller _controller)
    {
        controller = _controller;
    }

    //Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI") && curRaysastResult.gameObject == this.gameObject)
                return true;
        }
        return false;
    }


    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    private void Update()
    {
        if (!controller.isCursorLocked) return;

        bool isHit = false;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, LayerMask.GetMask("Paintable")))
        {
            distanceToSurface = hitInfo.distance;
            float mappedDistance = Mathf.Clamp(distanceToSurface, 0, maxDistance);
            float t = Mathf.InverseLerp(0, maxDistance, mappedDistance);
            float lerpedValue = Mathf.Lerp(controller.minFlowValue, controller.maxFlowValue, t);
            controller.SetPaintFlow(lerpedValue);

            if (!controller.lastDrawnGameObjects.Contains(hitInfo.collider.gameObject))
               { controller.lastDrawnGameObjects.Add(hitInfo.collider.gameObject); }

            isHit = true;
        }
        else
        {
            if (controller.IsDrawing())
            {
                controller.Trip();
            }

            isHit = false;

			distanceToSurface = -1;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(!IsPointerOverUIElement())
            {
                if (!isHit)
                {
                    ToastMessage.Instance.Show("Please move closer to the wall to paint");
                    distanceTextCountdownValue = distanceTextCountdown;
                }
                else
                {
                    controller.StartDrawing();
                }
            }

            
        }

		if (distanceTextCountdownValue <= 0)
		{
			ToastMessage.Instance.Hide();
		}
		else
		{
			distanceTextCountdownValue -= Time.deltaTime;
		}
	}
}
