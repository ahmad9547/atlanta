
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    public KeyCode colorKey;
    public FlexibleColorPicker colorPicker;
    public Controller controller1;

    private void Start()
    {
        EventManager.Instance.SetControllerEvent += SetController;
    }

    private void OnDisable()
    {
        EventManager.Instance.SetControllerEvent -= SetController;
    }

    private void SetController(Controller controller)
    {
        DataManager.Instance.SetController(controller);
        controller1 = controller;
    }

    private void Update()
    {
        if(Input.GetKeyUp(colorKey))
        {
            OnClick_ToggleColorPicker();
        }
    }

    public void OnClick_ToggleColorPicker()
    {
        if (controller1 == null)
        {
            return;
        }
        colorPicker.gameObject.SetActive(!colorPicker.gameObject.activeSelf);
        controller1.SetColor(colorPicker.color);

        if (colorPicker.gameObject.activeSelf)
        {
            controller1.ShowCursor();
        }
        else
        {
            controller1.HideCursor();
		}
	}
}