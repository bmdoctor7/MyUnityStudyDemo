using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrolling : MonoBehaviour
{
    [Header("移动设置")]
    public float maxMoveSpeed = 10f; // 摄像机最大移动速度
    public float edgeThreshold = 0.8f; // 屏幕边缘触发阈值 (0-1)，值越小触发区域越大
    public float smoothFactor = 5f; // 平滑移动因子，值越大移动越平缓
    
    [Header("缩放设置")]
    private float tarZoom;
    public float zoomSpeed = 1f;
    public float zoomSmooth = 10f;
    public float zoomMin,zoomMax;
    public Transform cameraImagePos;
    
    [Header("移动边界 (世界坐标)")]
    public bool enableBounds = false; // 是否启用移动边界限制
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -10f;
    public float maxY = 10f;
    
    
    private Camera mainCamera;
    private Vector3 targetPosition;
    
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        targetPosition = transform.position;
    }
    
    
    void Update()
    {
        HandleEdgeScrolling();
        SmoothMoveCamera();
        
        HandleMouseZoom();
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, tarZoom, zoomSmooth * Time.deltaTime);
    }
    
    
    void HandleEdgeScrolling()
    {
        // 获取鼠标在屏幕上的位置（坐标范围 0~1）[citation:1]
        Vector3 mouseViewportPos = mainCamera.ScreenToViewportPoint(Input.mousePosition);
        
        // 转换为以屏幕中心为原点的坐标（范围 -1~1）
        Vector2 moveInput = new Vector2(
            (mouseViewportPos.x - 0.5f) * 2f, // 将 0-1 转换为 -1 到 1
            (mouseViewportPos.y - 0.5f) * 2f
        );

        // 计算移动方向（只在接近边缘时触发）
        Vector2 moveDirection = Vector2.zero;

        // 检查X轴方向
        if (Mathf.Abs(moveInput.x) > edgeThreshold)
        {
            moveDirection.x = Mathf.Sign(moveInput.x) * Mathf.InverseLerp(edgeThreshold, 1f, Mathf.Abs(moveInput.x));
        }

        // 检查Y轴方向
        if (Mathf.Abs(moveInput.y) > edgeThreshold)
        {
            moveDirection.y = Mathf.Sign(moveInput.y) * Mathf.InverseLerp(edgeThreshold, 1f, Mathf.Abs(moveInput.y));
        }
        
        float zoomFactor = mainCamera.orthographicSize / zoomMax;
        // 计算目标位置
        Vector3 moveAmount = new Vector3(moveDirection.x, moveDirection.y, 0) * maxMoveSpeed* zoomFactor * Time.deltaTime;
        targetPosition += moveAmount;

        // 应用边界限制
        if (enableBounds)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
        }
    }
    
    
    void SmoothMoveCamera()
    {
        // 使用插值平滑移动摄像机
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.deltaTime);
    }
    
    
    public void HandleMouseZoom()
    {
        float scroll = Input.mouseScrollDelta.y;

        if(scroll != 0)
        {
            tarZoom -= scroll * zoomSpeed;
            tarZoom = Mathf.Clamp(tarZoom, zoomMin, zoomMax);
            //float b = (tarZoom + 3) / 6f;
            //cameraImagePos.localScale = new Vector3(b, b, 1);
        }
    }
    
    
    // 辅助方法：在Scene视图中绘制移动边界（仅用于调试）
    void OnDrawGizmosSelected()
    {
        if (enableBounds)
        {
            Gizmos.color = Color.green;
            Vector3 center = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, 0);
            Vector3 size = new Vector3(maxX - minX, maxY - minY, 0.1f);
            Gizmos.DrawWireCube(center, size);
        }
    }
}
