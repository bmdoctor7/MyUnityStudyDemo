using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyManager : SingletonMonoBase<EnemyManager>
{
    
    private EnemyManager(){}
    
    [Header("对象池设置")]
    public int maxPoolSize = 100;
    
    // 对象池管理
    private Dictionary<GameObject, IObjectPool<GameObject>> _enemyPools;
    private readonly List<GameObject> _activeEnemies = new List<GameObject>();

    // 场景展示用的容器
    private Transform poolsRoot; // 根: EnemyPools
    private Dictionary<GameObject, Transform> poolParents; // key: 预制体, value: 该池所在层级的空物体
    
    private void Awake()
    {
        InitializePools();
    }

    // 初始化对象池
    private void InitializePools()
    {
        _enemyPools = new Dictionary<GameObject, IObjectPool<GameObject>>();
        poolParents = new Dictionary<GameObject, Transform>();
        
        // 清理旧的根对象
        if (poolsRoot)
        {
            if (Application.isPlaying) Destroy(poolsRoot.gameObject);
            else DestroyImmediate(poolsRoot.gameObject);
        }
        poolsRoot = new GameObject("EnemyPools").transform;
        
        // 查找场景中所有可能用到的预制体
        var allSpawnZones = FindObjectsOfType<SpawnZone>();
        HashSet<GameObject> allPrefabs = new HashSet<GameObject>();

        foreach (var zone in allSpawnZones)
        {
            foreach (var spawnInfo in zone.spawnInfos)
            {
                if (spawnInfo.enemyPrefab)
                {
                    allPrefabs.Add(spawnInfo.enemyPrefab);
                }
            }
        }
        
        // 为（每种敌人）创建对象池
        foreach (var prefab in allPrefabs)
        {
            // 1) 为该池创建一个层级容器
            var parentGO = new GameObject($"{prefab.name}_Pool");
            var parentTf = parentGO.transform;
            parentTf.SetParent(poolsRoot, false);
            poolParents.Add(prefab, parentTf);
            
            var pool = new ObjectPool<GameObject>(
                createFunc: () => CreateEnemy(prefab),
                actionOnGet: (obj) => OnGetEnemy(obj),
                actionOnRelease: (obj) =>
                {
                    if(obj)
                        OnReleaseEnemy(obj);
                },
                actionOnDestroy: (obj) => Destroy(obj),
                defaultCapacity: 10,
                maxSize: maxPoolSize
            );
            
            _enemyPools.Add(prefab, pool);
        }
        
        Debug.Log($"初始化了 {_enemyPools.Count} 种怪物对象池");
    }
    GameObject CreateEnemy(GameObject prefab)
    {
        GameObject enemy = Instantiate(prefab);
        EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
        if (enemyScript)
        {
            //记录原始预制体以便重置属性
            enemyScript.originalPrefab = prefab;
        }
        
        // 初次创建时就放入对应池的容器下
        if (poolParents.TryGetValue(prefab, out var parent))
        {
            enemy.transform.SetParent(parent, false);
        }
        
        return enemy;
    }
    
    void OnGetEnemy(GameObject enemy)
    {
        // 保证仍在其池容器下
        var enemyScript = enemy.GetComponent<EnemyBase>();
        if (enemyScript && enemyScript.originalPrefab &&
            poolParents.TryGetValue(enemyScript.originalPrefab, out var parent))
        {
            // 保持父子关系（若外部代码改了父物体，这里纠正）
            if (enemy.transform.parent != parent)
                enemy.transform.SetParent(parent, true);
        }
        
        enemy.SetActive(true);
        if (!_activeEnemies.Contains(enemy))
            _activeEnemies.Add(enemy);
    }
    
    void OnReleaseEnemy(GameObject enemy)
    {
        // 回收后归位到对应池容器并禁用
        var enemyScript = enemy.GetComponent<EnemyBase>();
        if (enemyScript && enemyScript.originalPrefab &&
            poolParents.TryGetValue(enemyScript.originalPrefab, out var parent))
        {
            if (enemy.transform.parent != parent)
                enemy.transform.SetParent(parent, true);
        }
        
        enemy.SetActive(false);
        _activeEnemies.Remove(enemy);
    }

    // 公开的生成敌人方法
    public void SpawnEnemy(GameObject enemyPrefab, Vector3 position)
    {
        if (_enemyPools.TryGetValue(enemyPrefab, out var pool))
        {
            var enemy = pool.Get();
            enemy.transform.position = position;
        }
        else
        {
            Debug.LogWarning($"未找到预制体 {enemyPrefab.name} 的对象池");
        }
    }

    // 返回敌人到对象池
    public void ReturnEnemy(GameObject enemy, GameObject originalPrefab)
    {
        // 已经被回收或不在管理列表中，直接忽略，防止重复 Release
        if (!enemy || !_activeEnemies.Contains(enemy))
            return;
        
        if (_enemyPools.TryGetValue(originalPrefab, out var pool))
        {
            pool.Release(enemy);
        }
        else
        {
            // 无对应池，销毁并同步维护列表
            Destroy(enemy);
            _activeEnemies.Remove(enemy);
        }
    }


    // 生成所有区域的怪物（手动调用）
    public void SpawnAllZones()
    {
        var allSpawnZones = FindObjectsOfType<SpawnZone>();
        foreach (var zone in allSpawnZones)
        {
            zone.SpawnEnemies();
        }
    }

    // 生成指定标签区域的怪物
    public void SpawnZonesByTag(string tag)
    {
        var allSpawnZones = FindObjectsOfType<SpawnZone>();
        foreach (var zone in allSpawnZones)
        {
            if (zone.CompareTag(tag))
            {
                zone.SpawnEnemies();
            }
        }
    }


    // 清空所有怪物
    public void ClearAllEnemies()
    {
        foreach (var enemy in new List<GameObject>(_activeEnemies))
        {
            var enemyScript = enemy.GetComponent<EnemyBase>();
            if (enemyScript && enemyScript.originalPrefab)
            {
                ReturnEnemy(enemy, enemyScript.originalPrefab);
            }
            else
            {
                enemy.SetActive(false);
            }
        }
        _activeEnemies.Clear();
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    

}
