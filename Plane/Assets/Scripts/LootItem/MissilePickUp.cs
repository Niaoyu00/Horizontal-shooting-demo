using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePickUp : LootItem
{
    protected override void PickUp()
    {
        //ʰȡ֮����ҵ���������һ
        player.PickUpMissile();
        base.PickUp();
    }
}
