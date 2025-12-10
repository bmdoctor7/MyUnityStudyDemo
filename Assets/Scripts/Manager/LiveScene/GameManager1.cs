using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager1 : SingletonMonoBase<GameManager1>
{
    private GameManager1(){}
    
    const string BACKPACK_Data_Path = "BackpackData";
    const string Toolbar_Data_Path = "ToolbarData";
    
    
    
    
    
    //WARNING: 保存的是 ScriptableObject 引用类型的数据，需要深拷贝
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
        // 为了不破坏场景中已有的引用关系，保证所有地方仍然指向同一个背包
        InventoryData loadedBackpack = ScriptableObject.CreateInstance<InventoryData>();
        JsonUtility.FromJsonOverwrite(json, loadedBackpack);

        // 获取当前背包
        var currentBackpack = InventoryManager.Instance.backpack;
        if (!currentBackpack)
        {
            // 只有在当前背包为空，没有任何地方在引用它的前提下，才能安全的直接替换引用
            InventoryManager.Instance.backpack = loadedBackpack;
            return;
        }
        Debug.Log("加载背包数据成功，路径为："+fullPath);
        // 深拷贝 slotsList
        if (currentBackpack.slotsList == null)
            currentBackpack.slotsList = new List<SlotData>();
        else
            currentBackpack.slotsList.Clear(); //先清空，避免重复加载时数据叠加，方便逐一覆盖

        if (loadedBackpack.slotsList != null)
        {
            foreach (var slot in loadedBackpack.slotsList)
            {
                // SlotData为可序列化的普通类，可以直接复用
                currentBackpack.slotsList.Add(slot);
            }
        }
        // 若不再需要临时对象可销毁
        Destroy(loadedBackpack);
        
        // BackpackUI.Instance.RefreshAll();
    }
    
    public void LoadToolbarData()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, Toolbar_Data_Path);
        if (!File.Exists(fullPath)) return;

        string json = File.ReadAllText(fullPath);

        // 创建临时 ScriptableObject 实例
        InventoryData loadedToolbar = ScriptableObject.CreateInstance<InventoryData>();
        JsonUtility.FromJsonOverwrite(json, loadedToolbar);

        // 获取当前工具栏
        var currentToolbar = InventoryManager.Instance.toolbarData;
        if (!currentToolbar)
        {
            // 只有在当前工具栏为空，没有任何地方在引用它的前提下，才能安全的直接替换引用
            InventoryManager.Instance.backpack = loadedToolbar;
            return;
        }
        Debug.Log("加载工具栏数据成功，路径为："+fullPath);
        // 深拷贝 slotsList
        if (currentToolbar.slotsList == null)
            currentToolbar.slotsList = new List<SlotData>();
        else
            currentToolbar.slotsList.Clear();

        if (loadedToolbar.slotsList != null)
        {
            foreach (var slot in loadedToolbar.slotsList)
            {
                // SlotData为可序列化的普通类，可以直接复用
                currentToolbar.slotsList.Add(slot);
            }
        }
        // 若不再需要临时对象可销毁
        Destroy(loadedToolbar);
        
        // ToolBarUI.Instance.UpdateUI();
    }
    
    
    
    
    
}
