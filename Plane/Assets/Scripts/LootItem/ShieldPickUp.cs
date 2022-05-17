using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : LootItem
{
    [SerializeField] AudioData fullHealthPickUpSFX;//满血时拾取道具音效
    [SerializeField] int fullHealthScoreBonus = 200;//护盾满了给200分
    [SerializeField] float shieldBonus = 20f;//护盾回血量
    [SerializeField] float InvincibleTime = 3f;//无敌时长
    protected override void PickUp()
    {
        if (player.IsFullHealth)
        {//如果玩家血量是满的

            pickUpSFX = fullHealthPickUpSFX;//满血拾取音效
            lootMessage.text = $"SCORE + {fullHealthScoreBonus},Invincible + {InvincibleTime}s";
            player.PickShield(InvincibleTime);//无敌3s
            ScoreManager.Instance.Addscore(fullHealthScoreBonus);//奖励分数
        }
        else
        {
            pickUpSFX = defaultPickUpSFX;//默认拾取音效
            lootMessage.text = $"SHIELD + {shieldBonus}";
            player.RestoreHealth(shieldBonus);//回血
        }
        base.PickUp();
    }
}
