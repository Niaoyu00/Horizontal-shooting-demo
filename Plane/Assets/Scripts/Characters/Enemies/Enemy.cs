using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] int deathEnergyBonus = 30;//死亡能量奖励
    [SerializeField] int scorePoint = 100;
    [SerializeField] protected int healthFactor = 2;//血量因素

    LootSpawner lootSpawner;//战利品生成器

    protected virtual void Awake()
    {
        lootSpawner = GetComponent<LootSpawner>();
    }

    protected override void OnEnable()
    {
        SetHealth();
        base.OnEnable();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {//跟敌人碰撞
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(30);
            Die();
        }
    }

    public override void Die()
    {
        ScoreManager.Instance.Addscore(scorePoint);//积分
        PlayerEnergy.Instance.Obtain(deathEnergyBonus);//敌机死亡获取能量
        EnemyManager.Instance.RemoveFromList(gameObject);//从敌人列表中移除
        lootSpawner.Spawn(transform.position);//生成战利品 
        base.Die();
    }
    protected virtual void SetHealth()
    {//波数增加 敌人血量增多 跟boss波数关联
        maxHealth += (int) (EnemyManager.Instance.WaveNumber / healthFactor);

    }
}
