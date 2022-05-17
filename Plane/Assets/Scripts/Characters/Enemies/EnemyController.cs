using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("--------MOVE---------")]

    [SerializeField] float enemyMoveSpeed = 2f;

    [SerializeField] float moveRotationAngle = 25f;

    [Header("--------FIRE---------")]


    [SerializeField] protected AudioData[] projectilesLaunchSFX;//�����ӵ�������Ч

    [SerializeField] protected GameObject[] projectiles;//�ӵ�

    [SerializeField] protected Transform muzzle;//����λ��

    [SerializeField] protected ParticleSystem muzzleVFX;//ǹ����Ч

    [SerializeField] protected float minFireInterval;

    [SerializeField] protected float maxFireInterval;

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();//����ȴ��̶�֡

    protected float enemyPaddingX;

    float enemyPaddingY;

    protected Vector3 targetPosition;
    protected virtual void Awake()
    {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;//���ģ�ͳߴ� 
        enemyPaddingX = size.x / 2f;
        enemyPaddingY = size.y / 2f;
    }

    protected virtual void OnEnable()
    {//ÿ����������ʱ��ִ��onenable��
        StartCoroutine(nameof(RandomlyMovingCoroutine));//��������ھ�ͷ��
        StartCoroutine(nameof(RandomlyFireCoroutine));//�������
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator RandomlyMovingCoroutine()//��������ƶ�Э��
    {
        //���������ұ߾�ͷ��
        transform.position = ViewPort.Instance.RandomEnemySpawnPosition(enemyPaddingX, enemyPaddingY);
        //�����ƶ����ӿ��ұ߰볡
        targetPosition = ViewPort.Instance.RandomRightHalfPosition(enemyPaddingX, enemyPaddingY);
        while (gameObject.activeSelf)
        {
            //if ����δ����Ŀ��λ�� ��//Vector3.Distance��ȡ�������
            if (Vector3.Distance(transform.position, targetPosition) >= enemyMoveSpeed * Time.fixedDeltaTime)
            //���ڼ�Сֵ.//Mathf.Epsilon:����0���ӽ�0�ĸ���
            {
                // ����ǰ��Ŀ��λ�� //MoveTowards:�����ƶ� ������������:ÿ֡�ƶ������ֵ
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, enemyMoveSpeed * Time.fixedDeltaTime);

                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.right);
            }
            else
            {
                //else �ѵ�������µ�Ŀ��λ��
                targetPosition = ViewPort.Instance.RandomRightHalfPosition(enemyPaddingX, enemyPaddingY);
            }

            yield return waitForFixedUpdate;
        }
    }
    protected virtual IEnumerator RandomlyFireCoroutine()
    {//���������

        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));

            if (GameManager.GameState == GameState.GameOver)
            {
                yield break;//ֹͣЭ��
            }

            foreach (var projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);
            }
            AudioManager.Instance.PlayRandomSFX(projectilesLaunchSFX);
            muzzleVFX.Play();//��ѭ������ ���Բ���ֹͣ
        }
    }
}
