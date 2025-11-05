using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//每个格子的数据
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
    
    
    public bool IsEmpty()
    {
        return count == 0;
    }
    public bool CanAddItem()
    {
        return count < item.maxCount;
    }
    
    
    //获取该格子剩余空间（判断物品是否可以堆叠）
    public int GetFreeSpace()
    {
        return item.maxCount - count;
    }
    
    public void Add(int numToAdd = 1)
    {
        if (CanAddItem()) this.count += numToAdd;
        else Debug.LogError("该格子物品已满，无法添加");
        OnChange?.Invoke();//事件广播
    }
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
