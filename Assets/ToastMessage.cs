using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToastMessage : MonoBehaviour
{
    public static ToastMessage Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject toastObject;
    public TextMeshProUGUI messageText;

    public void Show(string text)
    {
        messageText.text = text;
        toastObject.SetActive(true);
    }

    public void Hide()
    {
        toastObject.SetActive(false);
    }
}