
using UnityEngine;

public class PlayerProjectile : Projectile
{
    TrailRenderer trail;//轨迹
    protected virtual void Awake()
    {

        trail = GetComponentInChildren<TrailRenderer>();
        if (moveDirection != Vector2.right)
        {
            //FromToRotation:根据传入的开始与结束两个方向返回一个旋转值 GetChild(0)获取粒子系统对象，使得特效跟轨迹旋转角度一致
            transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.right, moveDirection);
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    void OnDisable()
    {
        //玩家子弹被禁用时SetActive(false)，消除子弹轨迹
        trail.Clear();
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        PlayerEnergy.Instance.Obtain(PlayerEnergy.PRECENT);//每颗子弹击中敌人回1%能量
    }


}
