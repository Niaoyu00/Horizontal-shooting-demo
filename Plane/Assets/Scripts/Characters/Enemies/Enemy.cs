using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] int deathEnergyBonus = 30;//������������
    [SerializeField] int scorePoint = 100;
    [SerializeField] protected int healthFactor = 2;//Ѫ������

    LootSpawner lootSpawner;//ս��Ʒ������

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
    {//��������ײ
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(30);
            Die();
        }
    }

    public override void Die()
    {
        ScoreManager.Instance.Addscore(scorePoint);//����
        PlayerEnergy.Instance.Obtain(deathEnergyBonus);//�л�������ȡ����
        EnemyManager.Instance.RemoveFromList(gameObject);//�ӵ����б����Ƴ�
        lootSpawner.Spawn(transform.position);//����ս��Ʒ 
        base.Die();
    }
    protected virtual void SetHealth()
    {//�������� ����Ѫ������ ��boss��������
        maxHealth += (int) (EnemyManager.Instance.WaveNumber / healthFactor);

    }
}
