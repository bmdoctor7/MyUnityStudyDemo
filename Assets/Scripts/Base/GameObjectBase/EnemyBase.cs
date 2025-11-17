using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    
    [Header("基础属性")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public float moveSpeed = 2f;
    public float attackDamage = 10f;
    public float attackRange = 1.5f;
    public float attackFirstRange = 0f;

    public GameObject originalPrefab;
    
    public virtual void  MoveTowardsObject(string ObjectTag)
    {
        // 简单的移动逻辑
        Transform obj = GameObject.FindGameObjectWithTag(ObjectTag).transform;
        if (obj)
        {
            Vector3 direction = (obj.position - transform.position).normalized;
            transform.position += direction * Time.deltaTime * moveSpeed;
        }
    }
    
    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    // 死亡处理
    public virtual void Die()
    {
        if (EnemyManager.Instance && originalPrefab)
        {
            EnemyManager.Instance.ReturnEnemy(gameObject, originalPrefab);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
    
    
    
    
    
}
