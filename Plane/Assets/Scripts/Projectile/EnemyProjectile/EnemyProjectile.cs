using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    private void Awake()
    {
        if (moveDirection != Vector2.left)
        {
            //FromToRotation:根据传入的开始与结束两个方向返回一个旋转值
            transform.rotation = Quaternion.FromToRotation(Vector2.left, moveDirection);
        }
    }

}
