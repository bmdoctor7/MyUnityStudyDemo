using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum E_UILayer
{
    Bot,
    Mid,
    Top,
    Sys
}





public class UIManager : SingletonBase<UIManager>
{

    public Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private Transform _botlayer;
    private Transform _midLayer;
    private Transform _topLayer;

    public UIManager()
    {
        //加载Canvas预制体
        GameObject canvas = ResManager.Instance.Load<GameObject>("UI/Canvas");
        Transform canvasTransform = canvas.transform;
        GameObject.DontDestroyOnLoad(canvas);
        
        _botlayer = canvasTransform.Find("_botlayer");
        _midLayer = canvasTransform.Find("_midLayer");
        _topLayer = canvasTransform.Find("_topLayer");
        
        GameObject eventSystem = ResManager.Instance.Load<GameObject>("UI/EventSystem");
        GameObject.DontDestroyOnLoad(eventSystem);
    }

    public void ShowPanel<T>(string panelName, E_UILayer layer = E_UILayer.Top, UnityAction<T> callback = null) where T : BasePanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].ShowMe();
            if (callback != null)
                callback(panelDic[panelName] as T);
            return;
        }
        
        ResManager.Instance.LoadAsync<GameObject>("Prefab/UI/" + panelName, (obj) =>
        {
            Transform parent = _topLayer;
            switch (layer)
            {
                case E_UILayer.Bot:
                    parent = _botlayer;
                    break;
                case E_UILayer.Mid:
                    parent = _midLayer;
                    break;
                case E_UILayer.Top:
                    parent = _topLayer;
                    break;
            }
            obj.transform.SetParent(parent);
            
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            
            ((RectTransform)obj.transform).offsetMax = Vector2.zero;
            ((RectTransform)obj.transform).offsetMin = Vector2.zero;
            
            //获得预设体上的脚本组件
            T panel = obj.GetComponent<T>();
            
            callback?.Invoke(panel);
            panelDic.Add(panelName, panel);
        });
    }

    public void HidePanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].HideMe();
            GameObject.Destroy(panelDic[panelName].gameObject);
            panelDic.Remove(panelName);
        }
    }
    
    
    
    
    
    
    
    
    
    
    




}
