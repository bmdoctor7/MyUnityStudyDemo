using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonoBase<InventoryManager>
{
    
    private InventoryManager(){}
    
    private void Awake()
    {
        Init();
        DontDestroyOnLoad(gameObject);
    }
    
    private Dictionary<ItemType,ItemData> itemDataDic = new Dictionary<ItemType, ItemData>();

    [HideInInspector]
    public InventoryData backpack;
    [HideInInspector]
    public InventoryData toolbarData;
    
    //获取游戏开始前配置好（保存好）的物品数据
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

    //进一步封装获取物品数据的方法，提高健壮性
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
        if (!item) return;

        //若背包有相同物品且未满，则堆叠
        foreach(SlotData slotData in backpack.slotsList)
        {
            if (slotData.item == item && slotData.CanAddItem())
            {
                slotData.Add();return;
            }
        }

        //否则放入空格子
        foreach (SlotData slotData in backpack.slotsList)
        {
            if (slotData.count == 0)
            {
                slotData.AddItem(item);return;
            }
        }
        
        //无空格放入新种类物品，背包已满
        Debug.LogWarning("无法放入仓库，你的背包" + backpack + "已满。");
    }
    
    
    
    
    
}
