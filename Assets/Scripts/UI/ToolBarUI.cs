using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarUI : MonoBehaviour
{
    public List<ToolBarSlotUI> slotuiList;

    private ToolBarSlotUI selectedSlotUI;//控制当前选中的栏位

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
}
