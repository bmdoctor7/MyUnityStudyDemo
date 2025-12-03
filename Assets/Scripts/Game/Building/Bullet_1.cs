using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_1 : BulletBase
{
    public string enemyTag = "Enemy";
    
    private Transform _target;
    private EnemyBase _targetEnemy;   // 可为空，用于判定死亡
    
    private float _life;
    
    // 缓存飞行方向（单位向量）
    private Vector3 _moveDir;
    
    private FlightMode _mode = FlightMode.Homing;

    
    public void SetTarget(Transform target)
    {
        _target = target;
        _targetEnemy = target ? target.GetComponent<EnemyBase>() : null;
        _mode = FlightMode.Homing;
        _moveDir = Vector3.zero; // 将在首帧追踪时写入
    }
    private bool TargetLost()
    {
        if (!_target || !_target.gameObject.activeInHierarchy) return true;
        if (_targetEnemy)
        {
            // 若有血量，按血量判断死亡；没有则忽略此判定
            return _targetEnemy.currentHealth <= 0;
        }
        return false;
    }
    private void Update()
    {
        _life += Time.deltaTime;
        if (_life >= maxLifeTime)
        {
            BulletManager.ReleaseBullet(gameObject);
            return;
        }

        if (_mode == FlightMode.Homing)
        {
            if (TargetLost())
            {
                //Debug.Log("Target lost1");
                _mode = FlightMode.Straight;
                if (_moveDir.sqrMagnitude < 1e-6f)
                    _moveDir = DirFromZRotation(transform.eulerAngles.z); // 兜底
            }
            else
            {
                var pos = transform.position;
                var dir = _target.position - pos;
                var dist = dir.magnitude;

                if (dist > Mathf.Epsilon)
                {
                    var forward = dir / dist;
                    _moveDir = forward; // 持续缓存当前飞行方向

                    // 朝向插值（2D，Z 轴朝向）
                    float angle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
                    var targetRot = Quaternion.Euler(0f, 0f, angle);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateLerp * Time.deltaTime);

                    transform.position = pos + forward * speed * Time.deltaTime;
                    return;
                }
                else
                {
                    // 过近则改直线
                    Debug.Log("Target lost2");
                    _mode = FlightMode.Straight;
                }
            }
        }
        
        // 直线模式：沿最后一次缓存的方向前进
        if (_moveDir.sqrMagnitude < 1e-6f)
            _moveDir = DirFromZRotation(transform.eulerAngles.z);
        transform.position += _moveDir * speed * Time.deltaTime;
        
    }

    
    private static Vector3 DirFromZRotation(float zDeg)
    {
        float rad = zDeg * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
    }
    
    // 子弹需配置触发器碰撞体
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other) return;

        // 命中任意敌人即回收
        if (other.gameObject.CompareTag(enemyTag))
        {
            //Debug.Log("zc");
            other.gameObject.GetComponent<EnemyBase>().TakeDamage(damage);
            _life = 0;
            BulletManager.ReleaseBullet(this.gameObject);
        }
    }
}
