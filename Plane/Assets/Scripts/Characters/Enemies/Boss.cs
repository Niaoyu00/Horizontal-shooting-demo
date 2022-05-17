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
        healthBar = FindObjectOfType<BossHealthBar>();//初始化血条
        healthBarCanvas = healthBar.GetComponentInChildren<Canvas>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        healthBar.Initialize(health, maxHealth);//血条变化
        healthBarCanvas.enabled = true;//开启血条ui画布
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {//跟boss相撞
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(50);
            TakeDamage(10);
        }
    }
    public override void Die()
    {
        healthBarCanvas.enabled = false;//关闭死亡ui画布
        base.Die();
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthBar.UpdateStats(health, maxHealth);//更新血条状态
    }
    protected override void SetHealth()
    {//boss增强
        maxHealth += EnemyManager.Instance.WaveNumber * healthFactor;//血量增加 
    }
}
