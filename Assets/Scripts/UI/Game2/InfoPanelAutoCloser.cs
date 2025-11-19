using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoPanelAutoCloser : MonoBehaviour
{
    public BuildingInfoPanel panel;
    

    private void Update()
    {
        if (!panel || !panel.IsOpen) return;
        if (BuildingPlacementManager.Instance.IsDragging) return;

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            panel.Hide();
            TimeScaleController.Instance.ForceReset();
        }
    }
}
