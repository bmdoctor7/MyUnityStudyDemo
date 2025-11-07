using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoManager : SingletonBase<MonoManager>
{
    private MonoManager(){}//私有化构造函数，防止外部实例化
    
    private MonoController monoExecuter;
    public MonoController MonoExecuter
    {
        get
        {
            if (!monoExecuter)
            {
                GameObject go = new GameObject(typeof(MonoController).Name);
                monoExecuter = go.AddComponent<MonoController>();
            }
            return monoExecuter;  
            
        }
    }


    #region 协程管理
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return MonoExecuter.StartCoroutine(routine);
    }
    public void StopCoroutine(IEnumerator routine)
    {   
        if(routine!=null)
            monoExecuter.StopCoroutine(routine);
    }
    public void StopAllCoroutines()
    {
        monoExecuter.StopAllCoroutines();
    }
    #endregion


    #region 生命周期管理
    public void AddUpdateListener(UnityAction action)
    {
        MonoExecuter.AddUpdateListener(action);
    }
    public void RemoveUpdateListener(UnityAction action)
    {
        MonoExecuter.RemoveUpdateListener(action);
    }
    public void RemoveUpdateAllListener()
    {
        MonoExecuter.RemoveUpdateAllListener();
    }
    
    public void AddFixedUpdateListener(UnityAction action)
    {
        MonoExecuter.AddFixedUpdateListener(action);
    }
    public void RemoveFixedUpdateListener(UnityAction action)
    {
        MonoExecuter.RemoveFixedUpdateListener(action);
    }
    public void RemoveFixedUpdateAllListener()
    {
        MonoExecuter.RemoveFixedUpdateAllListener();
    }
    
    public void AddLateUpdateListener(UnityAction action)
    {
        MonoExecuter.AddLateUpdateListener(action);
    }
    public void RemoveLateUpdateListener(UnityAction action)
    {
        MonoExecuter.RemoveLateUpdateListener(action);
    }
    public void RemoveLateUpdateAllListener()
    {
        MonoExecuter.RemoveLateUpdateAllListener();
    }
    #endregion
    
    
    
    
    
    
    
    
    
    
    
    
    
}


public class MonoController : MonoBehaviour
{
    event UnityAction updateEvent;
    event UnityAction fixedUpdateEvent;
    event UnityAction lateUpdateEvent;
    
    
    private void Update()
    {
        updateEvent?.Invoke();
    }
    
    private void FixedUpdate()
    {
        fixedUpdateEvent?.Invoke();
    }

    private void LateUpdate()
    {
        lateUpdateEvent?.Invoke();
    }
    
    
    #region 添加移除监听
    public void AddUpdateListener(UnityAction action)
    {
        updateEvent += action;
    }

    public void RemoveUpdateListener(UnityAction action)
    {
        updateEvent -= action;
    }
    public void RemoveUpdateAllListener()
    {
        updateEvent = null;
    }
    
    public void AddFixedUpdateListener(UnityAction action)
    {
        fixedUpdateEvent += action;
    }

    public void RemoveFixedUpdateListener(UnityAction action)
    {
        fixedUpdateEvent -= action;
    }
    public void RemoveFixedUpdateAllListener()
    {
        fixedUpdateEvent = null;
    }

    public void AddLateUpdateListener(UnityAction action)
    {
        lateUpdateEvent += action;
    }
    public void RemoveLateUpdateListener(UnityAction action)
    {
        lateUpdateEvent -= action;
    }

    public void RemoveLateUpdateAllListener()
    {
        lateUpdateEvent = null;
    }
    

    #endregion
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}