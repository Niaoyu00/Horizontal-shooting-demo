using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    [SerializeField] float damage;

    [SerializeField] GameObject hitVFX;

    [SerializeField] protected AudioData[] hitSFX;//�ӵ�������Ч

    [SerializeField] protected float moveSpeed = 10f;

    [SerializeField] protected Vector2 moveDirection;//�ƶ����� protected�ɼ̳�

    protected GameObject target;//����׼Ŀ��

    protected virtual void OnEnable()
    {
        StartCoroutine(nameof(MoveDirectly));
    }



    IEnumerator MoveDirectly()
    {
        while (gameObject.activeSelf)
        {
            //transform.Translate����Ҫ��������
            //���ӵ����ض������ƶ�
            Move();
            yield return null;
        }
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {//�ж��Ƿ�ײ����character��ɫ�������
        if (collision.gameObject.TryGetComponent<Character>(out Character character))
        //Լ����if (collision.gameObject.GetComponent<Character>()),�µ�д����ʡ����
        {
            character.TakeDamage(damage);//����˺�
            // var contactPoint = collision.GetContact(0);//��ȡ��ײ�Ӵ���
            //PoolManager.Release(hitVFX, contactPoint.point,Quaternion.LookRotation(contactPoint.normal));
            //������ͷ�������Ч contactPoint.point:��ײ������
            PoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
            AudioManager.Instance.PlayRandomSFX(hitSFX);
            gameObject.SetActive(false);//���ã��ص������
        }
    }
    //�����๫��Ŀ��
    protected void SetTarget(GameObject target) => this.target = target;

    //���ӵ����ض������ƶ�
    public void Move() => transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
}
