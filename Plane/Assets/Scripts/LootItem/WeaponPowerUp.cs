using UnityEngine;

public class WeaponPowerUp : LootItem
{
    [SerializeField] AudioData fullPowerPickUpSFX;//������ʰȡ��Ч
    [SerializeField] int fullPowerScoreBonus = 200;//�������˸�200��

    protected override void PickUp()
    {
        if (player.IsFUllPower)
        {//��������������������

            pickUpSFX = fullPowerPickUpSFX;//������ʰȡ��Ч
            lootMessage.text = $"MAX Power ! SCORE + {fullPowerScoreBonus}";
            ScoreManager.Instance.Addscore(fullPowerScoreBonus);//��������
        }
        else
        {
            pickUpSFX = defaultPickUpSFX;//Ĭ��ʰȡ��Ч
            lootMessage.text = "POWER UP!";
            player.PowerUp();//�����������+1
        }
        base.PickUp();
    }
}
