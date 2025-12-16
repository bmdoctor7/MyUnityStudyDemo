using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPool : SingletonMonoBase<ShadowPool>
{
    public GameObject shadowPrefab;

    public int shadowCount;

    private Queue<GameObject> _availableObjects = new Queue<GameObject>();

    void Awake()
    {
        //初始化对象池
        FillPool();
        DontDestroyOnLoad(gameObject);
    }

    public void FillPool()
    {
        for (int i = 0; i < shadowCount; i++)
        {
            var newShadow = Instantiate(shadowPrefab);
            newShadow.transform.SetParent(transform);

            //取消启用,返回对象池
            ReturnPool(newShadow);
        }
    }

    public void ReturnPool(GameObject obj)
    {
        obj.SetActive(false);

        _availableObjects.Enqueue(obj);
    }

    public GameObject GetFromPool()
    {
        if (_availableObjects.Count == 0)
        {
            FillPool();
        }
        var outShadow = _availableObjects.Dequeue();

        outShadow.SetActive(true);

        return outShadow;
    }
}
