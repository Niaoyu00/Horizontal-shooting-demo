using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerProjectileOverdrive
{
    [SerializeField] AudioData targetAcquired;//��Ч

    [Header("===== SPEED CHANGE =====")]

    [SerializeField] float lowSpeed = 8f;//�����ٶ�

    [SerializeField] float highSpeed = 25f;//�����ٶ�

    [SerializeField] float variableSpeedDelay = 0.5f;//����ʱ��

    [Header("===== EXPLOSION =====")]

    [SerializeField] GameObject explosionVFX = null;//��Ч

    [SerializeField] AudioData explosionSFX = null;//��Ƶ

    [SerializeField] float explosionRadius = 3f;//��ը�뾶

    [SerializeField] float explosionDamage = 100f;//��ը�˺�

    [SerializeField] LayerMask enemyLayerMask = default;//���˲㼶����

    WaitForSeconds waitVariableSpeedDelay;

    protected override void Awake()
    {
        base.Awake();
        waitVariableSpeedDelay = new WaitForSeconds(variableSpeedDelay);//��ʼ��
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(nameof(VariableSpeedCoroutine));
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        //1���õ�����ը������Ч
        PoolManager.Release(explosionVFX, transform.position);
        //2��������Ч
        AudioManager.Instance.PlayRandomSFX(explosionSFX);
        //3����ɷ�Χ�˺� Physics2D.Overlap:�����ص�������������ײ��
        var colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayerMask);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(explosionDamage);
            }
        }

    }
    private void OnDrawGizmosSelected()
    {//������ը��Χ
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);//�����߿�����
    }

    IEnumerator VariableSpeedCoroutine()
    {//����Э�� 
        moveSpeed = lowSpeed;
        yield return waitVariableSpeedDelay;
        moveSpeed = highSpeed;
        if (target != null)
        {
            AudioManager.Instance.PlayRandomSFX(targetAcquired);
        }
    }
}
