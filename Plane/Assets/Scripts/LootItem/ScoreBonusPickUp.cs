using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBonusPickUp : LootItem
{
    [SerializeField] int scoreBonus;//�����ı�
    protected override void PickUp()
    {
        ScoreManager.Instance.Addscore(scoreBonus);//��������
        base.PickUp();
    }
}
