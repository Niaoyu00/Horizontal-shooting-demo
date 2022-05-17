using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeam : MonoBehaviour
{
    [SerializeField] float damage = 50f;
    [SerializeField] GameObject hitVFX;

    void OnCollisionStay2D(Collision2D collision)
    {//��ײ����ʱ���¼� ��OnCollisionEnter2D �ᵼ�¼������������

        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        //Լ����if (collision.gameObject.GetComponent<Player>()),�µ�д����ʡ����
        {
            player.TakeDamage(damage);//����˺�
            // var contactPoint = collision.GetContact(0);//��ȡ��ײ�Ӵ���
            //PoolManager.Release(hitVFX, contactPoint.point,Quaternion.LookRotation(contactPoint.normal));
            //������ͷ�������Ч contactPoint.point:��ײ������
            PoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
        }
    }
}
