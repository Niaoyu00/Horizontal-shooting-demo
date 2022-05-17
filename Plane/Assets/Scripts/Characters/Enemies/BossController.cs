using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    [SerializeField] float continuousFireDuration = 1.5f;//连续开火持续时间

    [Header("==== Player Detection ====")]

    [SerializeField] Transform playerDetectionTransfrom;//检测盒的变换组件
    [SerializeField] Vector3 playerDetectionSize;//检测盒的尺寸
    [SerializeField] LayerMask playerLayer;//玩家层遮罩

    [Header("==== Beam ====")]

    [SerializeField] float beamCooldownTime = 12f;//激光武器冷却时间12s
    [SerializeField] AudioData beamChargingSFX;//激光蓄力音效
    [SerializeField] AudioData beamLaunchSFX;//激光发射音效

    bool isBeamReady;//激光武器是否冷却

    int launchBeamID = Animator.StringToHash("launchBeam");//转化为哈希

    WaitForSeconds waitForContinuousFireInterval;//等待最小间隔时间
    WaitForSeconds waitForFireInterval;//等待最长间隔时间
    WaitForSeconds waitBeamCooldwonTime;//等待激光武器冷却时间


    List<GameObject> magazine;//弹夹列表 
    AudioData lauchSFX;//开火音效
    Animator animator;//获取动画
    Transform Playertransform;//玩家位置(用来追踪)

    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();

        waitForContinuousFireInterval = new WaitForSeconds(minFireInterval);//初始化等待时间
        waitForFireInterval = new WaitForSeconds(maxFireInterval);//初始化最长等待时间
        waitBeamCooldwonTime = new WaitForSeconds(beamCooldownTime);//初始化最长等待时间

        magazine = new List<GameObject>(projectiles.Length);//默认容量：子弹数组的长度

        Playertransform = GameObject.FindGameObjectWithTag("Player").transform;//获取玩家位置
    }

    protected override void OnEnable()
    {
        isBeamReady = false;//默认关闭激光
        muzzleVFX.Stop();//默认关闭开火特效
        StartCoroutine(nameof(BeamCooldownCoroutine));
        base.OnEnable();
    }

    void OnDrawGizmosSelected()
    {//画出检测范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerDetectionTransfrom.position, playerDetectionSize);
    }

    void ActiveBeamWeapon()
    {//激活激光武器
        isBeamReady = false;
        animator.SetTrigger(launchBeamID);//播放动画
        AudioManager.Instance.PlayRandomSFX(beamChargingSFX);//播放蓄力音效
    }
    void AnimationEvent_LaunchBeam()
    {
        AudioManager.Instance.PlayRandomSFX(beamLaunchSFX);//播放发射音效
    }
    void AnimationEvent_StopBeam()
    {
        StopCoroutine(nameof(ChasingPlayerCoroutine));//停用跟踪协程
        StartCoroutine(nameof(BeamCooldownCoroutine));//开启激光冷却
        StartCoroutine(nameof(RandomlyFireCoroutine));//再次开启随机开火协程

    }

    void LoadProjectiles()
    {//子弹装填函数

        magazine.Clear();

        //检测玩家在boss正前方 (位置，尺寸，角度，玩家遮罩)
        if (Physics2D.OverlapBox(playerDetectionTransfrom.position, playerDetectionSize, 0f, playerLayer))

        {//当玩家在boss正前方，发射 子弹1
            magazine.Add(projectiles[0]);//0号子弹
            lauchSFX = projectilesLaunchSFX[0];//0号音效音效
        }
        else
        {//发射2或者3号子弹 Random.value：随机0~1
            if (Random.value < 0.5f)
            {
                magazine.Add(projectiles[1]);//1号子弹
                lauchSFX = projectilesLaunchSFX[1];//1号子弹音效
            }
            else
            {
                for (int i = 2; i < projectiles.Length; i++)
                {
                    magazine.Add(projectiles[i]);
                }
                lauchSFX = projectilesLaunchSFX[2];//2号子弹音效
            }
        }
    }

    protected override IEnumerator RandomlyFireCoroutine()
    {//重写随机开火协程

        //游戏停止则退出协程
        while (isActiveAndEnabled)
        {
            if (GameManager.GameState == GameState.GameOver)
                yield break;
            if (isBeamReady)
            {
                ActiveBeamWeapon();//激活激光武器
                StartCoroutine(nameof(ChasingPlayerCoroutine));//开启追击玩家协程
                yield break;
            }
            yield return waitForFireInterval;//挂起等待最长开火时间
            yield return StartCoroutine(nameof(ContinuousFireCoroutine));//连续开火协程 并等待持续时间
        }
    }

    IEnumerator ContinuousFireCoroutine()
    { //检测玩家在面前,连续开火协程

        LoadProjectiles();//子弹装填函数
        muzzleVFX.Play();//播放枪口特效

        float continuousFireTimer = 0f;//计时
        while (continuousFireTimer < continuousFireDuration)
        {//if Timer<Duration boss不断开火
            foreach (var item in magazine)
            {
                PoolManager.Release(item, muzzle.position);//对象池发射子弹
            }
            continuousFireTimer += minFireInterval;//增加间隔值
            AudioManager.Instance.PlayRandomSFX(lauchSFX);//开火音效播放
            yield return waitForContinuousFireInterval;//挂起等待最短等待时长
        }
        muzzleVFX.Stop();
    }

    IEnumerator BeamCooldownCoroutine()
    {//激光武器冷却协程
        yield return waitBeamCooldwonTime;
        isBeamReady = true;
    }
    IEnumerator ChasingPlayerCoroutine()
    {//开激光之后追击玩家
        while (isActiveAndEnabled)
        {
            targetPosition.x = ViewPort.Instance.MaxX - enemyPaddingX;//boss x轴为视口最大x-bossx
            targetPosition.y = Playertransform.position.y;//y轴不变
            yield return null;
        }
    }
}
