using UnityEngine;

public class WeaponPowerUp : LootItem
{
    [SerializeField] AudioData fullPowerPickUpSFX;//满能量拾取音效
    [SerializeField] int fullPowerScoreBonus = 200;//能量满了给200分

    protected override void PickUp()
    {
        if (player.IsFUllPower)
        {//如果玩家武器威力是满的

            pickUpSFX = fullPowerPickUpSFX;//满能量拾取音效
            lootMessage.text = $"MAX Power ! SCORE + {fullPowerScoreBonus}";
            ScoreManager.Instance.Addscore(fullPowerScoreBonus);//奖励分数
        }
        else
        {
            pickUpSFX = defaultPickUpSFX;//默认拾取音效
            lootMessage.text = "POWER UP!";
            player.PowerUp();//玩家武器威力+1
        }
        base.PickUp();
    }
}
