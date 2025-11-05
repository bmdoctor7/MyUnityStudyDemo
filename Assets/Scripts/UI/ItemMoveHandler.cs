using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemMoveHandler : MonoBehaviour
{
    public static ItemMoveHandler Instance { get; private set; }
    
    private Image icon;
    //private SlotUI selectedSlotUI;
    private SlotData selectedSlotData;

    private PlayController player;

    private bool isCtrlDown = false;
    private void Awake()
    {
        Instance = this;
        icon = GetComponentInChildren<Image>();
        HideIcon();
        player = GameObject.FindAnyObjectByType<PlayController>();
    }
    
    private void Update()
    {
        if (icon.enabled)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                GetComponent<RectTransform>(), Input.mousePosition,
                null,
                out position);
            icon.GetComponent<RectTransform>().anchoredPosition = position;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                ThrowItem();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCtrlDown = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCtrlDown = false;
        }
        if (Input.GetMouseButtonDown(1))
        {
            ClearHandForced();
        }
    }
    
    private void ThrowItem()
    {
        if (selectedSlotData != null)
        {
            GameObject prefab = selectedSlotData.item.prefab;
            int count = selectedSlotData.count;
            if (isCtrlDown)
            {
                player.ThrowItem(prefab, 1);
                selectedSlotData.Reduce();
            }
            else
            {
                player.ThrowItem(prefab, count);
                selectedSlotData.Clear();
            }
            ClearHand();
        }
        
    }
    
    public void OnSlotClick(SlotUI slotui)
    {
        //判断手上是否为空
        
        //1-不为空
        if (selectedSlotData != null)
        {
            
            //1-1当前点击了一个空格子
            if (slotui.GetData().IsEmpty())
            {
                MoveToEmptySlot(selectedSlotData, slotui.GetData());
            }
            //1-2当前点击了一个非空格子
            else
            {
                //1-2-3点击了自身
                if (selectedSlotData == slotui.GetData()) return;
                //1-2-3点击了别的格子自身
                else
                { 
                    //类型一致
                    if(selectedSlotData.item == slotui.GetData().item)
                    {
                        MoveToSameTypeSlot(selectedSlotData, slotui.GetData());
                    }
                    //类型不一致
                    else
                    {
                        MoveToDiffTypeSlot(selectedSlotData, slotui.GetData());
                    }

                }

            }
        }
        //2-手上为空
        else
        {
            if (slotui.GetData().IsEmpty()) return;
            selectedSlotData = slotui.GetData();
            ShowIcon(selectedSlotData.item.sprite);
        }
        
    }
    
    //将物品放入空格子
    private void MoveToEmptySlot(SlotData fromData,SlotData toData)
    {
        if (isCtrlDown)
        {
            toData.AddItem(fromData.item);
            fromData.Reduce();
        }
        else
        {
            toData.MoveSlot(fromData);
            fromData.Clear();
        }
        ClearHand();
    }
    //物品放入非空格子（同类型）
    private void MoveToSameTypeSlot(SlotData fromData, SlotData toData)
    {
        if (isCtrlDown)
        {
            if (toData.CanAddItem())
            {
                toData.Add();
                fromData.Reduce();
            }
        }
        else
        {
            int freespace = toData.GetFreeSpace();
            //剩余空间不足以放下全部
            if (fromData.count > freespace)
            {
                toData.Add(freespace);
                fromData.Reduce(freespace);
            }
            //剩余空间足够
            else
            {
                toData.Add(fromData.count);
                fromData.Clear();
            }
        }
        ClearHand();
    }
    
    private void MoveToDiffTypeSlot(SlotData data1, SlotData data2)
    {
        ItemData item = data1.item;
        int count = data1.count;
        data1.MoveSlot(data2);

        data2.AddItem(item,count);
        ClearHandForced();
    }
    
    
    
    
    
    
    
    
    
    
    void HideIcon()
    {
        icon.enabled = false;
    }
    void ShowIcon(Sprite sprite)
    {
        icon.sprite = sprite;
        icon.enabled = true;
    }
    
    //清空手上的物品（放入物品后）
    void ClearHand()
    {
        if (selectedSlotData.IsEmpty())
        {
            HideIcon(); 
            selectedSlotData = null;
        }
    }
    //强制清空手上的物品（交换物品后）
    void ClearHandForced()
    {
        HideIcon();
        selectedSlotData = null;
    }
}
