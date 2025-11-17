using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//生成的每种怪物信息
[System.Serializable]
public class SpawnInfo
{
    public GameObject enemyPrefab;
    public int spawnCount;
}


public class SpawnZone : MonoBehaviour
{
    [Header("生成区域设置")]
    public Vector3 spawnArea = new Vector3(10f, 0f, 10f);
    
    [Header("生成怪物配置")]
    public List<SpawnInfo> spawnInfos = new List<SpawnInfo>();
    
    [Header("生成设置")]
    public bool spawnOnStart = false; // 是否刷怪
    public float spawnDelay = 0f;//刷怪间隔
    
    // 获取区域内的随机位置
    public Vector3 GetRandomPosition()
    {
        Vector3 center = transform.position;
        float x = Random.Range(-spawnArea.x / 2, spawnArea.x / 2);
        float y = Random.Range(-spawnArea.y / 2, spawnArea.y / 2);
        float z = Random.Range(-spawnArea.z / 2, spawnArea.z / 2);
        
        return center + new Vector3(x, y, z);
    }
    
    
    void Start()
    {
        if (spawnOnStart)
        {
            if (spawnDelay > 0)
            {
                Invoke("SpawnEnemies", spawnDelay);
            }
            else
            {
                SpawnEnemies();
            }
        }
    }
    
    
    // 生成该区域配置的所有怪物
    public void SpawnEnemies()
    {
        foreach (var spawnInfo in spawnInfos)
        {
            for (int i = 0; i < spawnInfo.spawnCount; i++)
            {
                Vector3 spawnPos = GetRandomPosition();
                //if(spawnInfo.enemyPrefab) Debug.Log("预制体正常");
                EnemyManager.Instance.SpawnEnemy(spawnInfo.enemyPrefab, spawnPos);
            }
        }
    }
    
    // 在Scene视图中显示生成区域
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, spawnArea);
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(transform.position, spawnArea);
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}
