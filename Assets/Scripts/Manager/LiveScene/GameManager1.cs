using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager1 : SingletonMonoBase<GameManager1>
{
    private GameManager1(){}
    
    const string BACKPACK_Data_Path = "BackpackData";
    const string Toolbar_Data_Path = "ToolbarData";
    
    
    
    
    

    public void SaveToolbarData()
    {
        SaveSystem.Instance.SaveByJson(Toolbar_Data_Path, InventoryManager.Instance.toolbarData);
    }
    
    public void SaveBackpackData()
    {
        SaveSystem.Instance.SaveByJson(BACKPACK_Data_Path, InventoryManager.Instance.backpack);
    }
    
    public void LoadBackpackData()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, BACKPACK_Data_Path);
        if (!File.Exists(fullPath)) return;

        string json = File.ReadAllText(fullPath);

        // 创建临时 ScriptableObject 实例
        InventoryData loaded = ScriptableObject.CreateInstance<InventoryData>();
        JsonUtility.FromJsonOverwrite(json, loaded);

        // 获取当前背包
        var current = InventoryManager.Instance.backpack;
        if (!current)
        {
            // 若系统允许直接替换引用
            InventoryManager.Instance.backpack = loaded;
            return;
        }
        Debug.Log("加载背包数据成功，路径为："+fullPath);
        // 深拷贝 slotsList
        if (current.slotsList == null)
            current.slotsList = new System.Collections.Generic.List<SlotData>();
        else
            current.slotsList.Clear();

        if (loaded.slotsList != null)
        {
            foreach (var slot in loaded.slotsList)
            {
                // 若 SlotData 也是 ScriptableObject 需决定是否复用还是克隆
                current.slotsList.Add(slot);
            }
        }
        // 若不再需要临时对象可销毁
        Destroy(loaded);
        
    }
    
    public void LoadToolbarData()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, Toolbar_Data_Path);
        if (!File.Exists(fullPath)) return;

        string json = File.ReadAllText(fullPath);

        // 创建临时 ScriptableObject 实例
        InventoryData loaded = ScriptableObject.CreateInstance<InventoryData>();
        JsonUtility.FromJsonOverwrite(json, loaded);

        // 获取当前工具栏
        var current = InventoryManager.Instance.toolbarData;
        if (!current)
        {
            // 若系统允许直接替换引用
            InventoryManager.Instance.backpack = loaded;
            return;
        }
        Debug.Log("加载工具栏数据成功，路径为："+fullPath);
        // 深拷贝 slotsList
        if (current.slotsList == null)
            current.slotsList = new List<SlotData>();
        else
            current.slotsList.Clear();

        if (loaded.slotsList != null)
        {
            foreach (var slot in loaded.slotsList)
            {
                // 若 SlotData 也是 ScriptableObject 需决定是否复用还是克隆
                current.slotsList.Add(slot);
            }
        }
        // 若不再需要临时对象可销毁
        Destroy(loaded);
    }
    
}
