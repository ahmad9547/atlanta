using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    private void Start()
    {
        //DontDestroyOnLoad(this);
    }

    public event Action<Controller> SetControllerEvent;
    public void SetControllerFunction(Controller controller)
    {
        SetControllerEvent?.Invoke(controller);
    }
    private void OnDestroy()
    {
        SetControllerEvent = null;
    }
}
