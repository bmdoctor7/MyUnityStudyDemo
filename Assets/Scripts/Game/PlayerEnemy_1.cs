using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemy_1 : EnemyBase
{
    
    private float _lastAttackTime;
    public float tattackFirstRange;
    private bool isInTrigger;
    
    private void Update()
    {
        MoveTowardsObject("Player");
    }

    private void FixedUpdate()
    {
        if (isInTrigger)
        {
            // 先消耗前摇时间（注意用 deltaTime）
            if (tattackFirstRange > 0f)
            {
                tattackFirstRange -= Time.fixedDeltaTime;
                return;
            }
            
            // 每个敌人独立冷却
            if (Time.time - _lastAttackTime >= attackRange )
            {
                var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayController>();
                if (player && player.currentHealth > 0)
                {
                    player.TakeDamage(attackDamage);
                    _lastAttackTime = Time.time;
                }
            }
        }
    }


    // 敌人被子弹击中
    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.CompareTag("Bullet"))
        // {
        //     TakeDamage(25);
        //     Destroy(other.gameObject);
        // }
        if (other.CompareTag("Player"))
        {
            // 每次开始接触玩家时，重置前摇倒计时
            tattackFirstRange = attackFirstRange;
            isInTrigger = true;
        }
    }
    
    // // 敌人攻击玩家（一定间隔攻击一次）
    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     if (!other.CompareTag("Player")) return;
    //     
    //     // 先消耗前摇时间（注意用 deltaTime）
    //     if (tattackFirstRange > 0f)
    //     {
    //         tattackFirstRange -= Time.fixedDeltaTime;
    //         return;
    //     }
    //     
    //     // 每个敌人独立冷却
    //     if (Time.time - _lastAttackTime >= attackRange )
    //     {
    //         var player = other.GetComponent<PlayController>();
    //         if (player && player.currentHealth > 0)
    //         {
    //             player.TakeDamage(attackDamage);
    //             _lastAttackTime = Time.time;
    //         }
    //     }
    // }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
        }
    }
}
