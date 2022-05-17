using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeam : MonoBehaviour
{
    [SerializeField] float damage = 50f;
    [SerializeField] GameObject hitVFX;

    void OnCollisionStay2D(Collision2D collision)
    {//碰撞进行时的事件 用OnCollisionEnter2D 会导致激光推着玩家走

        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        //约等于if (collision.gameObject.GetComponent<Player>()),新的写法节省性能
        {
            player.TakeDamage(damage);//造成伤害
            // var contactPoint = collision.GetContact(0);//获取碰撞接触点
            //PoolManager.Release(hitVFX, contactPoint.point,Quaternion.LookRotation(contactPoint.normal));
            //对象池释放命中特效 contactPoint.point:碰撞点坐标
            PoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
        }
    }
}
