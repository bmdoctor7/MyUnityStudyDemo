using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour
{
    
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();

    private void Awake()
    {
        FindChildControl<Button>();
        FindChildControl<Image>();
        FindChildControl<TextMeshProUGUI>();
        FindChildControl<Slider>();
        FindChildControl<Toggle>();
        FindChildControl<ScrollRect>();
    }
    
    /// <summary>
    /// 找到对应类型的子控件并存入字典
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void FindChildControl<T>() where T : UIBehaviour
    {
        T[] controls = this.GetComponentsInChildren<T>(true);
        foreach (T control in controls)
        {
            string controlName = control.gameObject.name;
            if (!controlDic.ContainsKey(controlName))
            {
                controlDic[controlName] = new List<UIBehaviour>();
            }
            controlDic[controlName].Add(control);
        }
    }
    
    /// <summary>
    /// 得到对应名字和类型的控件
    /// </summary>
    /// <param name="controlName">该组件的名字</param>
    /// <typeparam name="T">该组件的类型</typeparam>
    /// <returns></returns>
    protected T GetControl<T>(string controlName) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(controlName))
        {
            foreach (UIBehaviour control in controlDic[controlName])
            {
                if (control is T)
                {
                    return control as T;
                }
            }
        }
        return null;
    }
    
    
    public virtual void ShowMe(){}
    
    public virtual void HideMe(){}
    
    
    
    
    
    
    
    
    
    
}
