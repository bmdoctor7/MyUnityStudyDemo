using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacementManager : SingletonMonoBase<BuildingPlacementManager>
{
    private BuildingPlacementManager(){}
    
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask buildingMask;
    [SerializeField] private Color validColor = new Color(0f, 1f, 0f, 0.5f);
    [SerializeField] private Color invalidColor = new Color(1f, 0f, 0f, 0.5f);
    [SerializeField] private float placementRadius = 0.5f; // 放置重叠检测半径
    [SerializeField] private float planeZ = 0f;            // 2D 物体所处的 Z 平面

    private BuildingData placingData;
    private GameObject ghost;          // 建筑预览物
    private bool viewingOnly;          // 仅查看不放置
    private SpriteRenderer[] srs;      // 预览渲染器(2D)
    
    public bool IsDragging => dragging;
    private bool dragging;
    
    public void EnterViewingMode()
    {
        viewingOnly = true;
    }
    public void BeginDrag(BuildingData data)
    {
        // 退出查看模式并隐藏面板
        viewingOnly = false;
        var panel = Object.FindObjectOfType<BuildingInfoPanel>();
        if (panel && panel.IsOpen) panel.Hide();

        placingData = data;
        dragging = true;
        CreateGhost();
        UpdateGhostPosition();
    }

    public void EndDrag(bool releasedOverUI)
    {
        if (!dragging) return;

        bool legal = !releasedOverUI && IsLegalPosition();
        if (legal && EconomyManager.Instance.Spend(placingData.Cost))
        {
            var placed = Instantiate(placingData.Prefab, ghost.transform.position, Quaternion.identity);
            GameManager2.Instance.buildingsList.Add(placed);
        }

        Cancel();
    }

    public void UpdateGhostPosition()
    {
        if (!dragging || ghost == null) return;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit2D = Physics2D.GetRayIntersection(ray, Mathf.Infinity, groundMask);
        Vector3 pos = hit2D.collider ? (Vector3)hit2D.point
            : Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = planeZ;
        ghost.transform.position = pos;

        SetGhostColor(IsLegalPosition());
    }
    
    
    private void CreateGhost()
    {
        DestroyGhost();
        ghost = Instantiate(placingData.Prefab);
        srs = ghost.GetComponentsInChildren<SpriteRenderer>(true);

        foreach (var c in ghost.GetComponentsInChildren<Collider2D>(true)) c.enabled = false;
        var rb = ghost.GetComponentInChildren<Rigidbody2D>(true);
        if (rb) rb.simulated = false;
        foreach (var mb in ghost.GetComponentsInChildren<MonoBehaviour>(true))
            mb.enabled = false;

        SetGhostColor(false);
    }

    private void DestroyGhost()
    {
        if (ghost) Object.Destroy(ghost);
        ghost = null;
        srs = null;
    }

    private void Update()
    {
        // 查看模式：任意左键点击立即关闭并恢复
        if (viewingOnly)
        {
            if (Input.GetMouseButtonDown(0))
            {
                viewingOnly = false;
                TimeScaleController.Instance.ForceReset();
                Object.FindObjectOfType<BuildingInfoPanel>()?.Hide();
            }
            return;
        }
    }

    private bool IsLegalPosition()
    {
        if (!ghost) return false;
        // 2D 重叠检测: 建筑层上是否有碰撞体
        var hit = Physics2D.OverlapCircle(ghost.transform.position, placementRadius, buildingMask);
        return !hit;
    }

    private void SetGhostColor(bool legal)
    {
        if (srs == null) return;
        var c = legal ? validColor : invalidColor;
        foreach (var sr in srs) sr.color = c;
    }
    
    
    private void Cancel()
    {
        DestroyGhost();
        placingData = null;
        dragging = false;
        TimeScaleController.Instance.ForceReset();
    }
    
    
    
}
