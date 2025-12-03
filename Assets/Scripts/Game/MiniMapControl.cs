using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapControl : MonoBehaviour
{
    private Camera mainCamera;
    private float currentSize;
    private float mainSize;
    
    void Start()
    {
        if (!mainCamera)
        {
            mainCamera = Camera.main;
        }
        currentSize = GetComponent<Camera>().orthographicSize;
        mainSize = mainCamera.orthographicSize;
    }

    private void Update()
    {
        float basePercent = mainCamera.orthographicSize / mainSize;
        GetComponent<Camera>().orthographicSize = currentSize * basePercent;
    }
}
