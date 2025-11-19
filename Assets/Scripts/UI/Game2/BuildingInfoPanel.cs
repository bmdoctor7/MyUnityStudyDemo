using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 控制建筑信息点击后面板的显示与隐藏
/// </summary>
public class BuildingInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private TextMeshProUGUI costText;
    private BuildingData current;

    public void Show(BuildingData data)
    {
        current = data;
        nameText.text = data.DisplayName;
        descText.text = data.Description;
        costText.text = $"费用: {data.Cost}";
        root.SetActive(true);
    }

    public void Hide()
    {
        current = null;
        root.SetActive(false);
    }

    public bool IsOpen => root.activeSelf;
    
}
