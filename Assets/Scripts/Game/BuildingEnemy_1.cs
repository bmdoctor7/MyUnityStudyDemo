using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingEnemy_1 : EnemyBase
{
    private float _lastAttackTime;
    public float tattackFirstRange;
    private bool isInTrigger;
    public bool isMove = true;
    public GameObject obj;

    private void Start()
    {
        obj = FindObjectBuilding();
    }

    private void Update()
    {
        if(isMove)
            MoveTowardsObject("Building");
    }

    public override void MoveTowardsObject(string dir)
    {
        //TODO: 场景中没有建筑物时的处理
        if(!obj) return;
        // 简单的移动逻辑
        Vector3 direction = (obj.transform.position - transform.position).normalized;
        transform.position += direction * Time.deltaTime * moveSpeed;
    }

    public GameObject FindObjectBuilding()
    {
        if(GameManager2.Instance.buildingsList.Count == 0) return null;
        GameObject tempObj = GameManager2.Instance.buildingsList[0];
        if(!tempObj) return null;
        
        //找最近的建筑物
        foreach (GameObject nearestObj in GameManager2.Instance.buildingsList)
        {
            if (nearestObj)
            {
                if(Vector2.Distance(nearestObj.transform.position, transform.position) <
                   Vector2.Distance(tempObj.transform.position, transform.position))
                {
                    tempObj = nearestObj;
                }
            }
        }
        return tempObj;
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
                if(!obj) return;
                var tarBuilding = obj.gameObject.GetComponent<BuildingBase>();
                if (tarBuilding && tarBuilding.currentHealth > 0)
                {
                    tarBuilding.TakeDamage(attackDamage);
                    _lastAttackTime = Time.time;
                }
            }
        }
    }


    // 敌人被子弹击中
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(25);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Building"))
        {
            // 每次开始接触玩家时，重置前摇倒计时
            tattackFirstRange = attackFirstRange;
            isInTrigger = true;
            isMove = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Building"))
        {
            isInTrigger = false;
            isMove = true;
        }
    }
}
