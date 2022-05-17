using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileOverdrive : PlayerProjectile
{
    [SerializeField] ProjectileGuidanceSystem guidanceSystem;
    protected override void OnEnable()
    {
        //子弹目标为当前场景随机敌人 

        SetTarget(EnemyManager.Instance.RandomEnemy);
        transform.rotation = Quaternion.identity;

        if (target == null)
        {//正常朝前发射
            base.OnEnable();
        }
        else
        {
            //子弹追踪 启用子弹制导系统中的归巢协程
            StartCoroutine(guidanceSystem.HomingCoroutine(target));
        }

    }

}
