using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile_Missile : Projectile
{
    private void Awake()
    {
        //查找特定tag，返回对象
        SetTarget(GameObject.FindGameObjectWithTag("Player"));
    }
    protected override void OnEnable()
    {
        StartCoroutine(nameof(MoveDirectionCoroutine));
        base.OnEnable();
    }

    IEnumerator MoveDirectionCoroutine()
    {//移动方向协程
        //延迟一帧让引擎校准位置
        yield return null;
        //目标还活跃
        if (target.activeSelf)
        {
            //导弹追踪原理：玩家目标位置-当前子弹位置
            moveDirection = (target.transform.position - transform.position).normalized;
        }
    }
}
