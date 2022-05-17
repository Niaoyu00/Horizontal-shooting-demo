using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    BossHealthBar healthBar;

    Canvas healthBarCanvas;
    protected override void Awake()
    {
        base.Awake();
        healthBar = FindObjectOfType<BossHealthBar>();//��ʼ��Ѫ��
        healthBarCanvas = healthBar.GetComponentInChildren<Canvas>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        healthBar.Initialize(health, maxHealth);//Ѫ���仯
        healthBarCanvas.enabled = true;//����Ѫ��ui����
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {//��boss��ײ
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(50);
            TakeDamage(10);
        }
    }
    public override void Die()
    {
        healthBarCanvas.enabled = false;//�ر�����ui����
        base.Die();
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthBar.UpdateStats(health, maxHealth);//����Ѫ��״̬
    }
    protected override void SetHealth()
    {//boss��ǿ
        maxHealth += EnemyManager.Instance.WaveNumber * healthFactor;//Ѫ������ 
    }
}
