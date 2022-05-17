using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    [SerializeField] float damage;

    [SerializeField] GameObject hitVFX;

    [SerializeField] protected AudioData[] hitSFX;//子弹命中音效

    [SerializeField] protected float moveSpeed = 10f;

    [SerializeField] protected Vector2 moveDirection;//移动方向 protected可继承

    protected GameObject target;//可瞄准目标

    protected virtual void OnEnable()
    {
        StartCoroutine(nameof(MoveDirectly));
    }



    IEnumerator MoveDirectly()
    {
        while (gameObject.activeSelf)
        {
            //transform.Translate不需要依赖刚体
            //让子弹朝特定方向移动
            Move();
            yield return null;
        }
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {//判断是否撞到了character角色类的物体
        if (collision.gameObject.TryGetComponent<Character>(out Character character))
        //约等于if (collision.gameObject.GetComponent<Character>()),新的写法节省性能
        {
            character.TakeDamage(damage);//造成伤害
            // var contactPoint = collision.GetContact(0);//获取碰撞接触点
            //PoolManager.Release(hitVFX, contactPoint.point,Quaternion.LookRotation(contactPoint.normal));
            //对象池释放命中特效 contactPoint.point:碰撞点坐标
            PoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
            AudioManager.Instance.PlayRandomSFX(hitSFX);
            gameObject.SetActive(false);//禁用，回到对象池
        }
    }
    //向子类公开目标
    protected void SetTarget(GameObject target) => this.target = target;

    //让子弹朝特定方向移动
    public void Move() => transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
}
