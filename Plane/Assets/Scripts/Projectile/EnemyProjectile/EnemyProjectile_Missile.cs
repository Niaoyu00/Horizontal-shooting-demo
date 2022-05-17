using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile_Missile : Projectile
{
    private void Awake()
    {
        //�����ض�tag�����ض���
        SetTarget(GameObject.FindGameObjectWithTag("Player"));
    }
    protected override void OnEnable()
    {
        StartCoroutine(nameof(MoveDirectionCoroutine));
        base.OnEnable();
    }

    IEnumerator MoveDirectionCoroutine()
    {//�ƶ�����Э��
        //�ӳ�һ֡������У׼λ��
        yield return null;
        //Ŀ�껹��Ծ
        if (target.activeSelf)
        {
            //����׷��ԭ�����Ŀ��λ��-��ǰ�ӵ�λ��
            moveDirection = (target.transform.position - transform.position).normalized;
        }
    }
}
