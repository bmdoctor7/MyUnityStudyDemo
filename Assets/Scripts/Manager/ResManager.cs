using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResManager : SingletonBase<ResManager>
{
    private ResManager(){}

    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <param name="name">对象名字</param>
    /// <typeparam name="T">对象类型</typeparam>
    /// <returns>要加载的对象</returns>
    public T Load<T>(string name) where T : Object
    {
        T res = Resources.Load<T>(name);

        if (res is GameObject)
            return GameObject.Instantiate(res);
        else //例如Texture、AudioClip等资源
            return res;

    }

    /// <summary>
    /// 异步加载资源 —— 调用后物体需要等待一段时间才会被加载，所以要在方法中将物体传出去
    /// </summary>
    /// <param name="name"></param>
    /// <param name="callback"></param>
    /// <typeparam name="T"></typeparam>
    public void LoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        MonoManager.Instance.StartCoroutine(ReallyLoadAsync(name, callback));
    }
    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        ResourceRequest req = Resources.LoadAsync<T>(name);
        yield return req;
        
        if(req.asset is GameObject)
        {
            T res = GameObject.Instantiate(req.asset) as T;
            callback?.Invoke(res);
        }
        else
        {
            T res = req.asset as T;
            callback?.Invoke(res);
        }
    }
    
    
    
    
    
    
}
