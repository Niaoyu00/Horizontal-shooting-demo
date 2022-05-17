using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileOverdrive : PlayerProjectile
{
    [SerializeField] ProjectileGuidanceSystem guidanceSystem;
    protected override void OnEnable()
    {
        //�ӵ�Ŀ��Ϊ��ǰ����������� 

        SetTarget(EnemyManager.Instance.RandomEnemy);
        transform.rotation = Quaternion.identity;

        if (target == null)
        {//������ǰ����
            base.OnEnable();
        }
        else
        {
            //�ӵ�׷�� �����ӵ��Ƶ�ϵͳ�еĹ鳲Э��
            StartCoroutine(guidanceSystem.HomingCoroutine(target));
        }

    }

}
