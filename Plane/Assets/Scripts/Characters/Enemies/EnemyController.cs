using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("--------MOVE---------")]

    [SerializeField] float enemyMoveSpeed = 2f;

    [SerializeField] float moveRotationAngle = 25f;

    [Header("--------FIRE---------")]


    [SerializeField] protected AudioData[] projectilesLaunchSFX;//敌人子弹发射音效

    [SerializeField] protected GameObject[] projectiles;//子弹

    [SerializeField] protected Transform muzzle;//开火位置

    [SerializeField] protected ParticleSystem muzzleVFX;//枪口特效

    [SerializeField] protected float minFireInterval;

    [SerializeField] protected float maxFireInterval;

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();//挂起等待固定帧

    protected float enemyPaddingX;

    float enemyPaddingY;

    protected Vector3 targetPosition;
    protected virtual void Awake()
    {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;//获得模型尺寸 
        enemyPaddingX = size.x / 2f;
        enemyPaddingY = size.y / 2f;
    }

    protected virtual void OnEnable()
    {//每当对象被启用时会执行onenable。
        StartCoroutine(nameof(RandomlyMovingCoroutine));//随机生成在镜头外
        StartCoroutine(nameof(RandomlyFireCoroutine));//随机开火
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator RandomlyMovingCoroutine()//敌人随机移动协程
    {
        //生成在最右边镜头外
        transform.position = ViewPort.Instance.RandomEnemySpawnPosition(enemyPaddingX, enemyPaddingY);
        //敌人移动在视口右边半场
        targetPosition = ViewPort.Instance.RandomRightHalfPosition(enemyPaddingX, enemyPaddingY);
        while (gameObject.activeSelf)
        {
            //if 敌人未到达目标位置 。//Vector3.Distance获取两点距离
            if (Vector3.Distance(transform.position, targetPosition) >= enemyMoveSpeed * Time.fixedDeltaTime)
            //大于极小值.//Mathf.Epsilon:大于0，接近0的浮点
            {
                // 继续前往目标位置 //MoveTowards:对象移动 ，第三个参数:每帧移动量最大值
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, enemyMoveSpeed * Time.fixedDeltaTime);

                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.right);
            }
            else
            {
                //else 已到达，给它新的目标位置
                targetPosition = ViewPort.Instance.RandomRightHalfPosition(enemyPaddingX, enemyPaddingY);
            }

            yield return waitForFixedUpdate;
        }
    }
    protected virtual IEnumerator RandomlyFireCoroutine()
    {//随机开火功能

        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));

            if (GameManager.GameState == GameState.GameOver)
            {
                yield break;//停止协程
            }

            foreach (var projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);
            }
            AudioManager.Instance.PlayRandomSFX(projectilesLaunchSFX);
            muzzleVFX.Play();//非循环播放 所以不用停止
        }
    }
}
