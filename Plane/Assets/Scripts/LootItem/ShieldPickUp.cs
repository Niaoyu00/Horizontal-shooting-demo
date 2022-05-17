using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : LootItem
{
    [SerializeField] AudioData fullHealthPickUpSFX;//��Ѫʱʰȡ������Ч
    [SerializeField] int fullHealthScoreBonus = 200;//�������˸�200��
    [SerializeField] float shieldBonus = 20f;//���ܻ�Ѫ��
    [SerializeField] float InvincibleTime = 3f;//�޵�ʱ��
    protected override void PickUp()
    {
        if (player.IsFullHealth)
        {//������Ѫ��������

            pickUpSFX = fullHealthPickUpSFX;//��Ѫʰȡ��Ч
            lootMessage.text = $"SCORE + {fullHealthScoreBonus},Invincible + {InvincibleTime}s";
            player.PickShield(InvincibleTime);//�޵�3s
            ScoreManager.Instance.Addscore(fullHealthScoreBonus);//��������
        }
        else
        {
            pickUpSFX = defaultPickUpSFX;//Ĭ��ʰȡ��Ч
            lootMessage.text = $"SHIELD + {shieldBonus}";
            player.RestoreHealth(shieldBonus);//��Ѫ
        }
        base.PickUp();
    }
}
