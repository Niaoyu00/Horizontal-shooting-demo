using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGuidanceSystem : MonoBehaviour
{

    [SerializeField] Projectile projectile;

    [SerializeField] float minBallisticAngle = 50f;//��С�����Ƕ�

    [SerializeField] float maxBallisticAngle = 70f;//���

    float ballisiticAngle;//�洢�Ƕ����ֵ

    Vector3 targetDirection;
    // �ӵ��Ƶ�ϵͳ
    public IEnumerator HomingCoroutine(GameObject target)
    {//�鳲Э��

        ballisiticAngle = Random.Range(minBallisticAngle, maxBallisticAngle);

        while (gameObject.activeSelf)
        {//��ǰ�����Ծ
            if (target.activeSelf)//�ж�Ŀ���Ƿ��Ծ
            {
                //�ӵ�����Ŀ������ƶ�
                targetDirection = target.transform.position - transform.position;//Ŀ��λ�ü�ȥ��ǰλ������

                //Mathf.Rad2����ֵת�Ƕ� ��ȡԭ�㵽��(x,y)�ĽǶ�
                var angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                //Vector3.forward:z��
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                transform.rotation *= Quaternion.Euler(0f, 0f, ballisiticAngle);//�ӵ��ƶ�ʱ��Ťת�����Ƕ�

                //��Ŀ���ƶ�
                projectile.Move();
            }
            else
            {//�ӵ��̶������ƶ�
                projectile.Move();
            }
            yield return null;
        }
    }
}
