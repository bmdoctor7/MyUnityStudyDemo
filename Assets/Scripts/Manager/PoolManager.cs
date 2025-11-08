using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// 对象池中的数据结构
/// </summary>
public class PoolData
{
    public GameObject fatherObj; //对象挂载的父物体
    public List<GameObject> poolList; //对象池列表
        
    public PoolData(GameObject obj, GameObject poolObj)
    {
        //根据obj创建一个同名的父对象，并将其挂载到poolObj下
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.parent = poolObj.transform;

        poolList = new List<GameObject>() { };

        PushObj(obj);
    }

    //将对象放入对象池中的一类数据中
    public void PushObj(GameObject obj)
    {
        poolList.Add(obj);
        obj.transform.parent = fatherObj.transform;
        obj.SetActive(false);
    }

    public GameObject GetObj()
    {
        GameObject obj = null;
        obj = poolList[0];
        poolList.RemoveAt(0);
        obj.SetActive(true);
        obj.transform.parent = null;   
        return obj;
    }
}

public class PoolManager : SingletonBase<PoolManager>
{
    private PoolManager(){}
    
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();
    
    private GameObject poolObj;

    /// <summary>
    /// 外界需要某个对象时，从对象池中通过方法获取该对象
    /// </summary>
    /// <param name="name">需要生成对象的名字</param>
    /// <param name="callback">生成该对象的方法</param>
    public void GetObj(string name, UnityAction<GameObject> callback)
    {
        if(poolDic.ContainsKey(name) && poolDic[name].poolList.Count>0)
        {  
            callback?.Invoke(poolDic[name].GetObj());
        }
        else
        {
            //缓存池中没有该对象，异步加载该对象
            ResManager.Instance.LoadAsync<GameObject>(name, (res) =>
            {
                res.name = name;
                callback?.Invoke(res);
            });
        }
    }
    /// <summary>
    /// 外界需要某个对象时，从对象池中直接获取该对象
    /// </summary>
    /// <param name="name"></param>
    /// <returns>需要获取的对象</returns>
    public GameObject GetObj(string name)  
    {        
        GameObject obj = null;  
        if(poolDic.ContainsKey(name) && poolDic[name].poolList.Count>0)  
        {            
            obj = poolDic[name].GetObj();  
        }        
        else  
        {  
            //缓存池中没有该对象，Resources中直接加载该对象
            obj = GameObject.Instantiate(Resources.Load<GameObject>(name));  
            obj.name = name;  
        }        
        return obj;  
    }
    
    
    
    
    /// <summary>
    /// 外界“销毁”某个对象时，实际上是将其放入对象池中
    /// </summary>
    /// <param name="name">需要销毁的对象名字</param>
    /// <param name="obj">销毁的对象</param>
    public void PushObj(string name, GameObject obj)
    {
        if (!poolObj)
            poolObj = new GameObject("Pool");
        
        if (poolDic.ContainsKey(name))
            poolDic[name].PushObj(obj);
        else
            poolDic.Add(name, new PoolData(obj, poolObj));
    }
    
    //场景切换时清空对象池
    public void ClearPool()
    {
        poolDic.Clear();
        poolObj = null;
    }
    
    
    
    
}
