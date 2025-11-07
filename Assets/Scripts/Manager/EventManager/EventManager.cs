using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static Dictionary<EventType, Delegate> EventCenter = new Dictionary<EventType, Delegate>();


    #region 安全性判断

    //添加监听时需要的判断
    private static void OnListenerAdding(EventType eventType, Delegate callBack)
    {
        //确保事件类型存在
        if (!EventCenter.ContainsKey(eventType))
        {
            EventCenter.Add(eventType, null); // 创建新的事件条目
        }
        Delegate d = EventCenter[eventType]; //获取当前已注册的委托

        //类型一致性验证(参数一致)
        if (d != null && d.GetType() != callBack.GetType())
        {
            throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，当前事件所对应的委托是{1}，要添加的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
        }
    }

    //移除监听时需要的判断
    private static void OnListenerRemoving(EventType eventType, Delegate callBack)
    {
        // 确认事件类型存在
        if (EventCenter.ContainsKey(eventType))
        {
            Delegate d = EventCenter[eventType]; //获取当前已注册的委托

            // 确认委托不为空
            if (d == null)
            {
                throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", eventType));
            }
            // 类型一致性验证
            else if (d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，当前委托类型为{1}，要移除的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
            }
        }
        else
        {
            // 如果事件类型不存在，抛出异常
            throw new Exception(string.Format("移除监听错误：没有事件码{0}", eventType));
        }
    }

    //移除监听后需要的判断(移除后委托是否为空)
    private static void OnListenerRemoved(EventType eventType)
    {
        // 检查移除后委托是否为空
        if (EventCenter[eventType] == null)
        {
            EventCenter.Remove(eventType);// 如果为空，从字典中移除该事件条目
        }
    }

    #endregion

    #region 添加事件监听

    //no parameters
    public static void AddListener(EventType eventType, CallBack callBack)
    {
        //安全性检查
        OnListenerAdding(eventType, callBack);
        //委托合并
        EventCenter[eventType] = (CallBack)EventCenter[eventType] + callBack;
    }

    //Single parameters
    public static void AddListener<T>(EventType eventType, CallBack<T> callBack)
    {
        OnListenerAdding(eventType, callBack);
        EventCenter[eventType] = (CallBack<T>)EventCenter[eventType] + callBack;
    }

    #endregion

    #region 移除事件监听

    //no parameters
    public static void RemoveListener(EventType eventType, CallBack callBack)
    {
        OnListenerRemoving(eventType, callBack);// 安全检查
        EventCenter[eventType] = (CallBack)EventCenter[eventType] - callBack;// 移除委托
        OnListenerRemoved(eventType); // 清理检查
    }

    //single parameters
    public static void RemoveListener<T>(EventType eventType, CallBack<T> callBack)
    {
        OnListenerRemoving(eventType, callBack);
        EventCenter[eventType] = (CallBack<T>)EventCenter[eventType] - callBack;
        OnListenerRemoved(eventType);
    }

    #endregion

    #region 事件广播

    //no parameters
    public static void Broadcast(EventType eventType)
    {
        Delegate d;
        if (EventCenter.TryGetValue(eventType, out d))
        {
            CallBack callBack = d as CallBack;
            if (callBack != null)
            {
                callBack();
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
            }
        }
    }

    //single parameters
    public static void Broadcast<T>(EventType eventType, T arg)
    {
        Delegate d;
        if (EventCenter.TryGetValue(eventType, out d))
        {
            CallBack<T> callBack = d as CallBack<T>;
            if (callBack != null)
            {
                callBack(arg);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
            }
        }
    }

    #endregion
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}

