using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class BossEnemy : EnemyBase
{
    public Seeker seeker;
    private List<Vector3> pathPointsList = new List<Vector3>(); // 存储路径点的列表
    private int currentPathIndex = 0; // 当前路径点索引
    
    [Header("路径寻路设置")]
    public float pathUpdateInterval = 1f; // 路径更新间隔时间
    public float pathUpdateTimer = 0f; // 路径更新的计时器
    public float chaseDistance;
    
    
    
    [Header("攻击设置")]
     public float attackDistance;
    public float meleeAttackDamage = 20f;
    public float attackCooldownDuration = 1.5f; // 攻击冷却时间
    private bool isAttacking = true;
    public LayerMask playerLayerMask;
    
    public Transform playerTransform;
    public SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        seeker = GetComponent<Seeker>();
    }

    private void Update()
    {
        if(!playerTransform)return;
        
        float distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

        if (distanceToPlayer < chaseDistance)
        {
            AutoPath();
            
            if(pathPointsList == null || pathPointsList.Count == 0) return;

            float x = playerTransform.position.x - transform.position.x;
            if (x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
            
            if (distanceToPlayer < attackDistance)
            {
                if (isAttacking)
                {
                    isAttacking = false;
                    //TODO：攻击玩家
                    Debug.Log("Boss Enemy Attack Player!");
                    StartCoroutine(AttackCoolDownDuration());
                }
            }
            else
            {
                // 安全检查：确保有路径点且索引合法
                if (pathPointsList == null || pathPointsList.Count == 0) return;
                if (currentPathIndex < 0) currentPathIndex = 0;
                if (currentPathIndex >= pathPointsList.Count)
                {
                    // 如果索引超出范围，尝试重新生成路径或直接退出移动
                    GeneratePath(playerTransform.position);
                    return;
                }
                
                //Debug.Log("currentPathIndex:" + currentPathIndex);
                Vector2 direction = (pathPointsList[currentPathIndex] - transform.position).normalized;
                transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
            }
            
        }
        else
        {
            //TODO: 远离玩家或待机
        }
        
    }

    private void AutoPath()
    {
        pathUpdateTimer += Time.deltaTime;

        if (pathUpdateTimer >= pathUpdateInterval)
        {
            GeneratePath(playerTransform.position);
            pathUpdateTimer = 0f;
        }
        
        // 先检查 null 与 空
        if (pathPointsList == null || pathPointsList.Count <= 0 || currentPathIndex >= pathPointsList.Count)
        {
            GeneratePath(playerTransform.position);
        }
        // 到达当前点则推进索引
        else if (currentPathIndex < pathPointsList.Count)
        {
            if(Vector2.Distance(transform.position, pathPointsList[currentPathIndex]) <= 0.1f)
            {
                currentPathIndex++;
                if (currentPathIndex >= pathPointsList.Count)
                {
                    GeneratePath(playerTransform.position);
                }
            }
        }
        
    }
    
    private void GeneratePath(Vector3 target)
    {
        currentPathIndex = 0;
        // 立即清空旧路径，避免旧数据被误用
        if (pathPointsList == null) pathPointsList = new List<Vector3>();
        else pathPointsList.Clear();

        seeker.StartPath(transform.position, target, onPathComplete => {
            if (onPathComplete == null) return;
            if (onPathComplete.error) return;
            // 成功时安全赋值并重置索引
            pathPointsList = new List<Vector3>(onPathComplete.vectorPath);
            currentPathIndex = 0;
        });

    }


    // 近战攻击事件
    public void MeleeAttackAnimEvent()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackDistance, playerLayerMask);
        foreach (var hitCollider in hitColliders)
        {
            PlayController player = hitCollider.GetComponent<PlayController>();
            if (player)
            {
                player.TakeDamage(meleeAttackDamage);
            }
        }
    }

    IEnumerator AttackCoolDownDuration()
    {
        yield return new WaitForSeconds(attackCooldownDuration);
        isAttacking = true;
    }
    
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
