using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGuidanceSystem : MonoBehaviour
{

    [SerializeField] Projectile projectile;

    [SerializeField] float minBallisticAngle = 50f;//最小弹道角度

    [SerializeField] float maxBallisticAngle = 70f;//最大

    float ballisiticAngle;//存储角度随机值

    Vector3 targetDirection;
    // 子弹制导系统
    public IEnumerator HomingCoroutine(GameObject target)
    {//归巢协程

        ballisiticAngle = Random.Range(minBallisticAngle, maxBallisticAngle);

        while (gameObject.activeSelf)
        {//当前对象活跃
            if (target.activeSelf)//判断目标是否活跃
            {
                //子弹朝着目标持续移动
                targetDirection = target.transform.position - transform.position;//目标位置减去当前位置向量

                //Mathf.Rad2弧度值转角度 获取原点到点(x,y)的角度
                var angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                //Vector3.forward:z轴
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                transform.rotation *= Quaternion.Euler(0f, 0f, ballisiticAngle);//子弹移动时会扭转弹道角度

                //朝目标移动
                projectile.Move();
            }
            else
            {//子弹固定方向移动
                projectile.Move();
            }
            yield return null;
        }
    }
}
