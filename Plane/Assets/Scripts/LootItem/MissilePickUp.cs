using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePickUp : LootItem
{
    protected override void PickUp()
    {
        //拾取之后玩家导弹数量加一
        player.PickUpMissile();
        base.PickUp();
    }
}
