using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//每个背包或物品栏格子的数据
[System.Serializable]
public class SlotData 
{
    public ItemData item;
    public int count = 0;
    
    private Action OnChange;
    public void AddListener(Action OnChange)
    {
        this.OnChange = OnChange;
    }
    
    //判断该格子是否为空
    public bool IsEmpty()
    {
        return count == 0;
    }
    //判断该格子是否可以继续堆叠物品
    public bool CanAddItem()
    {
        return count < item.maxCount;
    }
    
    
    //获取该格子剩余空间（判断物品是否可以堆叠）
    public int GetFreeSpace()
    {
        return item.maxCount - count;
    }
    
    /// <summary>
    /// 向某个格子堆叠物品（背包中已有该物品）
    /// </summary>
    /// <param name="numToAdd">增加的数量</param>
    public void Add(int numToAdd = 1)
    {
        if (CanAddItem()) this.count += numToAdd;
        else Debug.LogError("该格子物品已满，无法添加");
        OnChange?.Invoke();//事件广播
    }
    
    /// <summary>
    /// 向某个格子添加物品（用于空格子）
    /// </summary>
    /// <param name="item">新增物品</param>
    /// <param name="count">新增物品数量</param>
    public void AddItem(ItemData item,int count =1)
    {
        this.item = item;
        this.count = count;
        OnChange?.Invoke();//事件广播
    }
    
    public void Reduce(int numToReduce = 1)
    {
        count -= numToReduce;
        if (count == 0)
        {
            Clear();
        }
        else
        {
            OnChange?.Invoke();
        }
    }
    public void Clear()
    {
        item = null;
        count = 0;
        OnChange?.Invoke();
    }
    
    public void MoveSlot(SlotData data)
    {
        this.item = data.item;
        this.count = data.count;
        OnChange?.Invoke();
    }
    
}
