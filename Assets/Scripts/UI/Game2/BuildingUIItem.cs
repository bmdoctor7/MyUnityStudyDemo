using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingUIItem : MonoBehaviour , IPointerClickHandler,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    public BuildingData data;
    [SerializeField] private Image icon;
    private BuildingInfoPanel infoPanel;

    private void Awake()
    {
        infoPanel = FindObjectOfType<BuildingInfoPanel>();
        if (icon && data) icon.sprite = data.Icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!data) return;
        infoPanel.Show(data);
        TimeScaleController.Instance.RequestSlow();
        BuildingPlacementManager.Instance.EnterViewingMode();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!data) return;
        if (!EconomyManager.Instance.CanAfford(data.Cost)) return;

        if (Mathf.Approximately(Time.timeScale, 1f)) TimeScaleController.Instance.RequestSlow();
        BuildingPlacementManager.Instance.BeginDrag(data);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (!BuildingPlacementManager.Instance.IsDragging) return;
        BuildingPlacementManager.Instance.UpdateGhostPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!BuildingPlacementManager.Instance.IsDragging) return;
        bool releasedOverUI = EventSystem.current.IsPointerOverGameObject();
        BuildingPlacementManager.Instance.EndDrag(releasedOverUI);
    }
    
    
    
}
