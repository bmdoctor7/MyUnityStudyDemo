using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour,IPointerClickHandler
{
    public int index;
    private SlotData data;

    public Image iconImage;
    public TextMeshProUGUI countText;


    public void SetData(SlotData newData)
    {
        this.data = newData;
        if (data != null)
        {
            //事件监听，只要数据变化就更新UI
            data.AddListener(OnDataChange);
            UpdateUI();
        }
        else
        {
            ClearUI();
        }
        
        
    }
    //UI更新事件
    private void OnDataChange()
    {
        UpdateUI();
    }
    //UI更新
    private void UpdateUI()
    {
        if (!data.item)
        {
            iconImage.enabled = false;
            countText.text = "";
        }
        else
        {
            iconImage.enabled = true;
            iconImage.sprite = data.item.sprite;
            countText.text = data.count >= 1 ? data.count.ToString() : "";
        }
    }
    private void ClearUI()
    {
        iconImage.enabled = false;
        countText.text = "";
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        ItemMoveHandler.Instance.OnSlotClick(this);
    }
    
    //获取该格子数据（封装）
    public SlotData GetData()
    {
        return data;
    }
    
}


