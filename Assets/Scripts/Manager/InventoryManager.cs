using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance{get; private set;}
    private void Awake()
    {
        Instance = this;
        Init();
    }
    
    
    
    
    private Dictionary<ItemType,ItemData> itemDataDic = new Dictionary<ItemType, ItemData>();

    [HideInInspector]
    public InventoryData backpack;
    [HideInInspector]
    public InventoryData toolbarData;
    
    //获取物品数据
    private void Init()
    {
        ItemData[] itemDatas = Resources.LoadAll<ItemData>("Data");
        foreach (ItemData itemData in itemDatas)
        {
            itemDataDic.Add(itemData.type, itemData);
        }
        
        backpack = Resources.Load<InventoryData>("Data/Backpack");
        toolbarData = Resources.Load<InventoryData>("Data/ToolBar");
        
    }

    //进一步封装获取物品数据的方法，外部调用更便捷
    private ItemData GetItemData(ItemType itemType)
    {
        ItemData itemData;
        bool isSuccess = itemDataDic.TryGetValue(itemType, out itemData);
        if (isSuccess) return itemData;
        else
        {
            Debug.LogError("没有该物品数据，物品类型为："+itemType);
            return null;
        }
    }
    
    //添加物品到背包
    public void AddToBackpack(ItemType type)
    {
        ItemData item = GetItemData(type);
        if (item == null) return;

        foreach(SlotData slotData in backpack.slotsList)
        {
            if (slotData.item == item && slotData.CanAddItem())
            {
                slotData.Add();return;
            }
        }

        foreach (SlotData slotData in backpack.slotsList)
        {
            if (slotData.count == 0)
            {
                slotData.AddItem(item);return;
            }
        }

        Debug.LogWarning("无法放入仓库，你的背包" + backpack + "已满。");
    }
    
    
    
    
    
}
