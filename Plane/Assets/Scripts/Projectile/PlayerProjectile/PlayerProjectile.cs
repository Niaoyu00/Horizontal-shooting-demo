
using UnityEngine;

public class PlayerProjectile : Projectile
{
    TrailRenderer trail;//�켣
    protected virtual void Awake()
    {

        trail = GetComponentInChildren<TrailRenderer>();
        if (moveDirection != Vector2.right)
        {
            //FromToRotation:���ݴ���Ŀ�ʼ������������򷵻�һ����תֵ GetChild(0)��ȡ����ϵͳ����ʹ����Ч���켣��ת�Ƕ�һ��
            transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.right, moveDirection);
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    void OnDisable()
    {
        //����ӵ�������ʱSetActive(false)�������ӵ��켣
        trail.Clear();
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        PlayerEnergy.Instance.Obtain(PlayerEnergy.PRECENT);//ÿ���ӵ����е��˻�1%����
    }


}
