using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletManager : SingletonMonoBase<BulletManager>
{
    private BulletManager(){}

    [Serializable]
    public class PoolConfig
    {
        public GameObject prefab;
        public int defaultCapacity = 100;
        public int maxSize = 100;
    }

    [Header("可选: 在 Inspector 中预注册子弹池")]
    public List<PoolConfig> prewarmPools = new();

    // 按 prefab 分组的对象池
    private readonly Dictionary<GameObject, ObjectPool<GameObject>> _pools = new();
    // 实例 -> 所属对象池，便于归还
    private readonly Dictionary<GameObject, ObjectPool<GameObject>> _instanceToPool = new();

    private void Awake()
    {
        // 预先创建配置的对象池
        foreach (var cfg in prewarmPools)
        {
            if (cfg == null || cfg.prefab == null) continue;
            GetOrCreatePool(cfg.prefab, cfg.defaultCapacity, cfg.maxSize);
        }
    }

    // 对外静态便捷方法
    public static GameObject SpawnBullet(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        => Instance.Spawn(prefab, position, rotation, parent);

    public static void ReleaseBullet(GameObject instance)
        => Instance.Despawn(instance);

    // 生成子弹
    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (!prefab) Debug.Log("BulletManager: Spawn called with null prefab!");

        var pool = GetOrCreatePool(prefab);
        var go = pool.Get();

        if (!go)
        {
            //Debug.LogWarning("BulletManager.Spawn: 从对象池取得的实例为 null, 将尝试重新实例化一个.");
            // 兜底: 直接新建 (不进池), 防止调用方逻辑中断
            var fallback = Instantiate(prefab, position, rotation);
            fallback.transform.SetParent(parent ? parent : transform, false);
            fallback.transform.SetPositionAndRotation(position, rotation);
            return fallback;
        }
        
        // 变换与父级
        var t = go.transform;
        t.SetParent(parent ? parent : transform, false);
        t.SetPositionAndRotation(position, rotation);

        return go;
    }

    // 回收
    public void Despawn(GameObject instance)
    {
        if (!instance) return;

        if (_instanceToPool.TryGetValue(instance, out var pool))
        {
            //Debug.Log("BulletManager: Despawn called!");
            pool.Release(instance);
        }
        // else
        // {
        //     // 未受管实例, 直接销毁以避免泄漏
        //     Debug.LogWarning("BulletManager.Despawn: 非池管理对象被回收, 执行 Destroy.");
        //     Destroy(instance);
        // }
    }

    // 预热若干个对象到池中
    public void Prewarm(GameObject prefab, int count)
    {
        var pool = GetOrCreatePool(prefab);
        var temp = new List<GameObject>(count);
        for (int i = 0; i < count; i++) temp.Add(pool.Get());
        foreach (var go in temp) pool.Release(go);
    }

    // 获取或创建指定 prefab 的对象池
    private ObjectPool<GameObject> GetOrCreatePool(GameObject prefab, int defaultCapacity = 10, int maxSize = 100)
    {
        if (_pools.TryGetValue(prefab, out var pool)) return pool;

        // 延迟捕获: createFunc 调用发生在构造之后, 此时 pool 已赋值
        pool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                var go = Instantiate(prefab);
                _instanceToPool[go] = pool;
                go.SetActive(false);
                return go;
            },
            actionOnGet: go =>
            {
                if(go)
                    go.SetActive(true);
            },
            actionOnRelease: go =>
            {
                if(go)
                    go.SetActive(false);
                else
                {
                    Debug.Log("BulletManager: Releasing " + go.name);
                }
            },
            actionOnDestroy: go =>
            {
                _instanceToPool.Remove(go);
                //Destroy(go);
            },
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );

        _pools[prefab] = pool;
        return pool;
    }
    
    
    
    
}
