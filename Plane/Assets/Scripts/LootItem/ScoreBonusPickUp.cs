using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBonusPickUp : LootItem
{
    [SerializeField] int scoreBonus;//分数文本
    protected override void PickUp()
    {
        ScoreManager.Instance.Addscore(scoreBonus);//分数增加
        base.PickUp();
    }
}
