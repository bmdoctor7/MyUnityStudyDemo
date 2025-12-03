using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_AttackTower : BuildingBase
{
    [Header("引用")]
    public AttackRangeDetector rangeDetector;
    public Transform firePoint;
    public GameObject bulletPrefab;
    
    private float _nextFireTime;
    
    
    private void Start()
    {
        UpdateHpui();
        rangeDetector = this.gameObject.GetComponent<AttackRangeDetector>();
    }


    private void FixedUpdate()
    {
        if (Time.time < _nextFireTime) return;
        if (!rangeDetector || !firePoint || !bulletPrefab) return;

        var target = rangeDetector.GetClosestTarget();
        if (!target) return;

        // 面向目标
        var dir = (target.position - firePoint.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var rot = Quaternion.Euler(0, 0, angle);

        // 生成子弹并设定目标
        var go = BulletManager.SpawnBullet(bulletPrefab, firePoint.position, rot);
        var bullet = go.GetComponent<Bullet_1>();
        if (bullet)
        {
            bullet.SetTarget(target);
        }

        _nextFireTime = Time.time + attackInterval;
    }
}
