using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Exp_Pool : SingletonMonoBase<Exp_Pool>
{
    private Exp_Pool(){}
    
    public GameObject expPrefab;
    public int maxPoolSize = 50;

    private IObjectPool<GameObject> expPool;
    private Transform poolParent;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        // 创建层级容器
        poolParent = new GameObject("ExpPool").transform;
        poolParent.parent = this.transform;

        // 创建对象池
        expPool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(expPrefab);
                obj.transform.parent = poolParent;
                return obj;
            },
            actionOnGet: (obj) =>
            {
                obj.SetActive(true);
            },
            actionOnRelease: (obj) =>
            {
                obj.SetActive(false);
                obj.transform.parent = poolParent;
            },
            actionOnDestroy: (obj) =>
            {
                Destroy(obj);
            },
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: maxPoolSize
        );
    }

    public GameObject GetExp()
    {
        return expPool.Get();
    }

    public void ReleaseExp(GameObject exp)
    {
        expPool.Release(exp);
    }
    
    public bool isEmpty()
    {
        if(expPool == null || expPool.CountInactive == 0)
            return true;
        return false;
    }
    
}
