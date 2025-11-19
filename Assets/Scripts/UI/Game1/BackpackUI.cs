using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BackpackUI : SingletonMonoBase<BackpackUI>
{
    
    const string BACKPACK_Data_Path = "BackpackData";
    
    private GameObject parentUI;
    
    public List<SlotUI> slotuiList;
    
    private void Awake()
    {
        parentUI = transform.Find("ParentUI").gameObject;
    }
    
    
    
    private void Start()
    {
        InitUI();
        UpdateUI();
    }
    //初始化背包的每个格子UI，保证物品添加顺序正确
    void InitUI()
    {
        slotuiList = new List<SlotUI>(new SlotUI[21]);  
        SlotUI[] slotuiArray = transform.GetComponentsInChildren<SlotUI>();

        foreach(SlotUI slotUI in slotuiArray)
        {
            slotuiList[slotUI.index] = slotUI;
        }
    }
    //实时更新背包里每个格子的UI显示
    public void UpdateUI()
    {
        List<SlotData> slotdataList = InventoryManager.Instance.backpack.slotsList;
        
        for(int i = 0; i < slotdataList.Count; i++)
        {
            slotuiList[i].SetData(slotdataList[i]);
        }
    }
    
    public void RefreshAll()
    {
        var backpack = InventoryManager.Instance.backpack;
        if (!backpack || backpack.slotsList == null) return;

        for (int i = 0; i < slotuiList.Count; i++)
        {
            if (i < backpack.slotsList.Count)
                slotuiList[i].SetData(backpack.slotsList[i]);
            else
                slotuiList[i].SetData(null);
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleUI();
        }
    }
    //关闭按钮
    public void OnCloseClick()
    {
        ToggleUI();
    }
    //切换UI显示状态
    private void ToggleUI()
    {
        parentUI.SetActive(!parentUI.activeSelf);
    }





    public void SaveBackpackData()
    {
        SaveSystem.Instance.SaveByJson(BACKPACK_Data_Path, InventoryManager.Instance.backpack);
    }

    public void LoadBackpackData()
    {
        LoadInventory();
        RefreshAll();
    }
    void LoadInventory()
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
    
    
    
    
    
    
    
}
