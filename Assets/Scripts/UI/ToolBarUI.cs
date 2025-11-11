using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ToolBarUI : MonoBehaviour
{
    public List<ToolBarSlotUI> slotuiList;

    private ToolBarSlotUI selectedSlotUI;//控制当前选中的栏位

    const string Toolbar_Data_Path = "ToolbarData";
    
    // Start is called before the first frame update
    void Start()
    {
        InitUI();
    }

    private void Update()
    {
        ToolbarSelectControl();

    }
    public ToolBarSlotUI GetSelectedSlotUI()
    {
        return selectedSlotUI;
    }

    void InitUI()
    {
        slotuiList = new List<ToolBarSlotUI>(new ToolBarSlotUI[9]);//初始化每个格子
        ToolBarSlotUI[] slotuiArray = transform.GetComponentsInChildren<ToolBarSlotUI>();//获取每个格子的数据

        foreach (ToolBarSlotUI slotUI in slotuiArray)
        {
            slotuiList[slotUI.index] = slotUI;//按顺序存入list
        }

        UpdateUI();
    }
    
    public void UpdateUI()
    {
        List<SlotData> slotdataList = InventoryManager.Instance.toolbarData.slotsList;//获取所有栏位数据

        for (int i = 0; i < slotdataList.Count; i++)
        {
            slotuiList[i].SetData(slotdataList[i]);//更新每个栏位UI
        }
    }
    
    //数字快捷键选择栏位
    void ToolbarSelectControl()
    {
        for (int i = (int)KeyCode.Alpha1; i <= (int)KeyCode.Alpha9; i++)
        {
            if (Input.GetKeyDown((KeyCode)i))
            {
                //若之前有选中的，取消高亮
                if (selectedSlotUI != null)
                {
                    selectedSlotUI.UnHighlight();
                }
                int index = i - (int)KeyCode.Alpha1;
                selectedSlotUI = slotuiList[index];
                selectedSlotUI.Highlight();
            }
        }
    }
    
    
    
    
    public void SaveToolbarData()
    {
        SaveSystem.Instance.SaveByJson(Toolbar_Data_Path, InventoryManager.Instance.toolbarData);
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
        
        UpdateUI();
    }
    
    
    
    
}
