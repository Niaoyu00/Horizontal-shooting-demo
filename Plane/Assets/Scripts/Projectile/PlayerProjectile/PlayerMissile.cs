using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerProjectileOverdrive
{
    [SerializeField] AudioData targetAcquired;//音效

    [Header("===== SPEED CHANGE =====")]

    [SerializeField] float lowSpeed = 8f;//慢速速度

    [SerializeField] float highSpeed = 25f;//高速速度

    [SerializeField] float variableSpeedDelay = 0.5f;//变速时间

    [Header("===== EXPLOSION =====")]

    [SerializeField] GameObject explosionVFX = null;//特效

    [SerializeField] AudioData explosionSFX = null;//音频

    [SerializeField] float explosionRadius = 3f;//爆炸半径

    [SerializeField] float explosionDamage = 100f;//爆炸伤害

    [SerializeField] LayerMask enemyLayerMask = default;//敌人层级遮罩

    WaitForSeconds waitVariableSpeedDelay;

    protected override void Awake()
    {
        base.Awake();
        waitVariableSpeedDelay = new WaitForSeconds(variableSpeedDelay);//初始化
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(nameof(VariableSpeedCoroutine));
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        //1，让导弹爆炸生成特效
        PoolManager.Release(explosionVFX, transform.position);
        //2，播放音效
        AudioManager.Instance.PlayRandomSFX(explosionSFX);
        //3，造成范围伤害 Physics2D.Overlap:物理重叠函数，返回碰撞体
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
    {//画出爆炸范围
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);//画出线框球体
    }

    IEnumerator VariableSpeedCoroutine()
    {//变速协程 
        moveSpeed = lowSpeed;
        yield return waitVariableSpeedDelay;
        moveSpeed = highSpeed;
        if (target != null)
        {
            AudioManager.Instance.PlayRandomSFX(targetAcquired);
        }
    }
}
