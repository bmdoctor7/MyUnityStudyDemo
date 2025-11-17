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
    private Dictionary<GameObject, IObjectPool<GameObject>> enemyPools;
    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Awake()
    {
        InitializePools();
    }

    // 初始化对象池
    public void InitializePools()
    {
        enemyPools = new Dictionary<GameObject, IObjectPool<GameObject>>();

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
            var pool = new ObjectPool<GameObject>(
                createFunc: () => CreateEnemy(prefab),
                actionOnGet: (obj) => OnGetEnemy(obj),
                actionOnRelease: (obj) => OnReleaseEnemy(obj),
                actionOnDestroy: (obj) => Destroy(obj),
                defaultCapacity: 10,
                maxSize: maxPoolSize
            );
            
            enemyPools.Add(prefab, pool);
        }
        
        Debug.Log($"初始化了 {enemyPools.Count} 种怪物对象池");
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
        return enemy;
    }
    
    void OnGetEnemy(GameObject enemy)
    {
        enemy.SetActive(true);
        activeEnemies.Add(enemy);
    }
    
    void OnReleaseEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        activeEnemies.Remove(enemy);
    }

    // 公开的生成敌人方法
    public void SpawnEnemy(GameObject enemyPrefab, Vector3 position)
    {
        if (enemyPools.TryGetValue(enemyPrefab, out var pool))
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
        if (enemyPools.TryGetValue(originalPrefab, out var pool))
        {
            pool.Release(enemy);
        }
        else
        {
            Destroy(enemy);
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
        foreach (var enemy in new List<GameObject>(activeEnemies))
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
        activeEnemies.Clear();
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    

}
