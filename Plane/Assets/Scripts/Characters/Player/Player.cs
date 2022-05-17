using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]//添加刚体组件
public class Player : Character
{
    #region FIELDS

    [SerializeField] Statsbar_HUD statsbar_HUD;

    [SerializeField] bool regenerateHealth = true;//是否再生

    [SerializeField] float HealthregenerateTime;//再生时间

    [SerializeField, Range(0f, 1f)] float HealthregenerateTimePercent;//再生百分比

    [SerializeField] GameObject invincibleVFX;//无敌特效

    [Header("--------INPUT--------")]
    [SerializeField] PlayInput input;

    [Header("--------MOVE--------")]
    [SerializeField] private float moveSpeed = 6f;

    [SerializeField] float accelerationTime = 3f;//加速时间

    [SerializeField] float decelerationTime = 3f;//减速时间

    [SerializeField] float moveRotationAngle = 50f;//移动旋转角度

    [Header("--------FIRE--------")]
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject projectile1;
    [SerializeField] GameObject projectile2;
    [SerializeField] GameObject projectileOverdrive;

    [SerializeField] ParticleSystem muzzleVFX;//枪口特效

    [SerializeField] Transform muzzleTop;//上枪口
    [SerializeField] Transform muzzleMiddle;//中枪口
    [SerializeField] Transform muzzleBottom;//下枪口

    [SerializeField] AudioData projectileSFX;//子弹发射音效

    [SerializeField, Range(0, 2)] int weaponPower = 0;//武器威力 Range()取值范围

    [SerializeField] float fireInterval = 0.2f;

    [Header("--------DODGE--------")]

    [SerializeField] AudioData dodgeSFX;//获取闪避音效

    [SerializeField, Range(0, 100)] int dodgeEnergyCost = 25;//闪避能量消耗值

    [SerializeField] float maxRoll = 720;//最大滚转角度

    [SerializeField] float rollSpeed = 360;//最大滚转角度

    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);//闪避时最小缩放值

    [Header("--------OVERDRIVE--------")]

    [SerializeField] int overdriveDodgeFactor = 2;

    [SerializeField] float overdriveSpeedFactor = 1.2f;

    [SerializeField] float overdriveFireFactor = 1.2f;

    [Header("--------AUDIO TUNING--------")]

    [SerializeField] Slider sliderDodge;//获取闪避音效调整ui 
    [SerializeField] Slider sliderProjectile;//获取子弹发射音效调整ui
    [SerializeField] Slider sliderDie;//获取玩家死亡音效调整ui



    bool isOverdriving = false;//是否开启爆发

    bool isDodging = false;//闪避中

    float dodgeDuration;//闪避持续时间

    float currentRoll;//当前滚转角度

    float paddingX;//需要偏移的量(值为模型x长度的一半)

    float paddingY;//需要偏移的量(值为模型y长度的一半)

    float t;

    int TakeDamageNum = 0;//受击次数

    readonly float slowMotionDuration = 1f;//初始化子弹时间 持续时间 

    [SerializeField] readonly float InvincibleTime = 1f;//角色激光 无敌持续时间

    WaitForSeconds waitForOverdriveFireInterval;//等待能量爆发时的开火间隔

    WaitForSeconds waitForFireInterval;//等待开火间隔

    WaitForSeconds waitHealthRegenerateTime;//等待生命值再生的时间

    WaitForSeconds waitInvincibleTime;//无敌时间

    Coroutine moveC;//用来赋值，方便停用带参数的协程

    Coroutine HealthRegenerateC;//用来赋值，方便停用带参数的协程

    new Rigidbody2D rigidbody;//获取刚体

    new Collider2D collider;//获取碰撞

    Vector2 previousVelocity;
    Vector2 moveDirection;//移动方向
    Quaternion previousRotation;

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    MissileSystem missile;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        missile = GetComponent<MissileSystem>();

        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;//获得模型尺寸 
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;

        dodgeDuration = maxRoll / rollSpeed;// 最大旋转角/翻滚速度=持续时间
        rigidbody.gravityScale = 0f;//重力设为0


        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval /= overdriveFireFactor);
        waitHealthRegenerateTime = new WaitForSeconds(HealthregenerateTime);
        waitInvincibleTime = new WaitForSeconds(InvincibleTime);

        TakeDamageNum = 0;//受击次数默认为0
    }
    void Start()
    {
        statsbar_HUD.Initialize(health, maxHealth);
        input.EnableGamePlayInput();//激活动作表

        StartCoroutine(AudioManager.Instance.AudioTuningCoroutine(dodgeSFX, sliderDodge));//开启闪避音量调整协程[音效对象,ui进度条对象]
        StartCoroutine(AudioManager.Instance.AudioTuningCoroutine(projectileSFX, sliderProjectile));//开启子弹发射音量调整协程
        StartCoroutine(AudioManager.Instance.AudioTuningCoroutine(deathSFX, sliderDie));//玩家死亡音效调整


    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    private void Update()
    {
        StartCoroutine(nameof(MoveRangeLimatationCoroutine));//启动位移限制携程(限制在视口内)

    }
    protected override void OnEnable()
    {
        base.OnEnable();
        //订阅事件
        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
        input.onDodge += Dodge;
        input.onOverdrive += Overdrive;
        input.onLaunchMissile += LaunchMissile;

        PlayerOverdrive.on += OverdriveOn;
        PlayerOverdrive.off += OverdriveOff;

    }
    private void OnDisable()
    {//删除事件
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
        input.onDodge -= Dodge;
        input.onOverdrive -= Overdrive;
        input.onLaunchMissile -= LaunchMissile;

        PlayerOverdrive.on -= OverdriveOn;
        PlayerOverdrive.off -= OverdriveOff;
    }
    #endregion

    #region PROPERTIES

    public bool IsFullHealth => health == maxHealth;//当前血量是否为满
    public bool IsFUllPower => weaponPower == 2;//判断武器威力是否为2


    #endregion

    #region HEALTH
    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        statsbar_HUD.UpdateStats(health, maxHealth);
    }
    public override void TakeDamage(float damage)
    {
        TakeDamageNum++;
        base.TakeDamage(damage);
        if (TakeDamageNum == 3)
        {//被击中3次后降低武器等级
            PowerDown();//降低武器等级
            TakeDamageNum = 0;
        }

        statsbar_HUD.UpdateStats(health, maxHealth);//hudui血条更新
        TimeController.Instance.BulletTime(0.5f);//子弹时间

        if (gameObject.activeSelf)
        {
            Move(moveDirection);
            StartCoroutine(InvincibleCoroutine());//无敌状态 1s
            if (regenerateHealth)//如果回血开关开启
            {
                if (HealthRegenerateC != null)
                {//如果不为空，则停用
                    StopCoroutine(HealthRegenerateC);
                }//启用自回血协程
                HealthRegenerateC = StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenerateTime, HealthregenerateTimePercent));
            }
        }
    }
    public override void Die()
    {
        GameManager.onGameOver?.Invoke();
        GameManager.GameState = GameState.GameOver;//游戏状态更新
        statsbar_HUD.UpdateStats(0f, maxHealth);
        base.Die();
    }
    #endregion

    #region FIRE
    private void Fire()
    {
        muzzleVFX.Play();//播放枪口特效
        //StartCoroutine("FireCoroutine");
        StartCoroutine(nameof(FireCoroutine));//获取名称 字符串格式

    }
    private void StopFire()
    {
        muzzleVFX.Stop();//关闭枪口特效
        //StopCoroutine("FireCoroutine");
        StopCoroutine(nameof(FireCoroutine));
    }
    IEnumerator FireCoroutine()
    {

        while (true)
        {   //用instantiate方法生成子弹
            //switch (weaponPower)
            //{
            //    case 0:
            //        Instantiate(projectile, muzzleMiddle.position, Quaternion.identity);
            //        break;
            //    case 1:
            //        Instantiate(projectile1, muzzleTop.position, Quaternion.identity);
            //        Instantiate(projectile1, muzzleBottom.position, Quaternion.identity);
            //        break;
            //    case 2:
            //        Instantiate(projectile, muzzleTop.position, Quaternion.identity);
            //        Instantiate(projectile1, muzzleMiddle.position, Quaternion.identity);
            //        Instantiate(projectile2, muzzleBottom.position, Quaternion.identity);
            //        break;
            //    default:
            //        break;
            //}

            switch (weaponPower)
            {//对象池生成子弹
                case 0://生成之前判断是否是爆发状态，若是则返回爆发子弹 否则就用旧的
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);
                    break;
                case 1:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleTop.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleBottom.position);
                    break;
                case 2:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile, muzzleTop.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile2, muzzleBottom.position);
                    break;
                default:
                    break;
            }
            AudioManager.Instance.PlayRandomSFX(projectileSFX);//音效开启
            //判断是否爆发中，是的话执行爆发开火间隔(更短)，否执行原本的正常开火间隔
            yield return isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;

        }
    }

    IEnumerator InvincibleCoroutine()
    {//受伤无敌1s
        collider.isTrigger = true;//碰撞体开关
        invincibleVFX.SetActive(true);//无敌特效开关
        yield return waitInvincibleTime;
        collider.isTrigger = false;
        invincibleVFX.SetActive(false);
    }
    IEnumerator ShieldCoroutine(float s)
    {//获取护盾,无敌5s
        collider.isTrigger = true;//碰撞体开关
        invincibleVFX.SetActive(true);
        yield return new WaitForSeconds(s);
        collider.isTrigger = false;
        invincibleVFX.SetActive(false);

    }
    public void PickShield(float s)
    {//获取护盾时 无敌s秒
        StartCoroutine(ShieldCoroutine(s));
    }

    #endregion

    #region MOVE
    private void Move(Vector2 moveInput)
    {
        //先停止之前的加速携程再运行。
        if (moveC != null)
        {
            StopCoroutine(moveC);
        }
        //特定轴上的旋转角度 moveInput.y 区间[-1.1]
        Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);

        moveDirection = moveInput.normalized;
        //输入信号二维向量的值*速度，得到移动量
        //rigidbody.velocity = moveInput * moveSpeed; //刚体的速度值
        moveC = StartCoroutine(MoveCoroutine(accelerationTime, moveDirection * moveSpeed, moveRotation));//缓慢加速


    }
    private void StopMove()
    {
        //先停止之前的减速携程再运行。
        if (moveC != null)
        {
            StopCoroutine(moveC);
        }
        moveDirection = Vector2.zero;
        //rigidbody.velocity = Vector2.zero;
        moveC = StartCoroutine(MoveCoroutine(decelerationTime, moveDirection, Quaternion.identity));//减速
        StopCoroutine(nameof(MoveRangeLimatationCoroutine));//停止位移限制携程
    }

    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation)
    {//加速减速
        t = 0f;
        previousVelocity = rigidbody.velocity;//rigidbody.velocity会持续改变，所以先存储再用作运算
        previousRotation = transform.rotation;//会持续改变，所以先存储再用作运算
        //while (t < 1f)
        //{
        //    t += Time.fixedDeltaTime / time;
        //    rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, moveVelocity, t );//移动 t / time是一个0~1的值
        //    transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, t );//旋转
        //    yield return null;
        //}
        while (t < time)
        {
            t += Time.fixedDeltaTime;
            rigidbody.velocity = Vector2.Lerp(previousVelocity, moveVelocity, t / time);//移动 t / time是一个0~1的值
            transform.rotation = Quaternion.Lerp(previousRotation, moveRotation, t / time);//旋转
            yield return waitForFixedUpdate;//挂起等待下一次更新
        }
    }
    IEnumerator MoveRangeLimatationCoroutine()
    {
        //位移限制携程
        while (true)
        {
            //限制玩家不超出视口
            transform.position = ViewPort.Instance.PlayerMoveablePosition(transform.position, paddingX, paddingY);

            yield return null;
        }
    }
    #endregion

    #region DODGE
    void Dodge()
    {
        if (isDodging || !PlayerEnergy.Instance.IsEnough(dodgeEnergyCost))
            return;
        StartCoroutine(nameof(DodgeCoroutine));
    }

    IEnumerator DodgeCoroutine()
    {
        isDodging = true;//正在翻滚
        currentRoll = 0f;//当前旋转角度归零
        AudioManager.Instance.PlayRandomSFX(dodgeSFX);
        //能量值消耗 25
        PlayerEnergy.Instance.Use(dodgeEnergyCost);
        //闪避时玩家无敌
        collider.isTrigger = true;
        var scale = transform.localScale;
        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);//子弹时间
        //闪避时玩家沿x轴旋转
        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;//旋转
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);

            if (currentRoll < maxRoll / 2f)
            {//旋转到一半时，飞机缩放
                scale -= (Time.deltaTime / dodgeDuration) * Vector3.one;
                //Debug.Log("缩小：" + scale);
            }
            else
            {
                scale += (Time.deltaTime / dodgeDuration) * Vector3.one;
                //因为上下移动导致旋转角判断不对，导致复原会比旧的飞机更大，因此加一个判断强制还原成1倍。
                if (scale.x > 1 && scale.y > 1 && scale.z > 1)
                {
                    scale = Vector3.one;
                }
                //Debug.Log("复原：" + scale);

            }
            transform.localScale = scale;
            yield return null;
        }
        //缩放玩家模型以达到向远处飞行的效果
        collider.isTrigger = false;
        isDodging = false;
    }

    #endregion

    #region OVERDRIVE
    private void Overdrive()
    {//能量值不满时 返回
        if (!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX))
            return;
        else
        {
            PlayerOverdrive.on.Invoke();//开启能量爆发
        }
    }
    private void OverdriveOn()
    {//开启能量爆发
        isOverdriving = true;
        dodgeEnergyCost *= overdriveDodgeFactor;//闪避(防御型能力)能量值所需消耗增加（乘以系数）
        moveSpeed *= overdriveSpeedFactor;//移速增加（乘以系数）
        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);//子弹时间
    }
    private void OverdriveOff()
    {//关闭能量爆发
        isOverdriving = false;
        dodgeEnergyCost /= overdriveDodgeFactor;//恢复能力
        moveSpeed /= overdriveSpeedFactor;//恢复能力
    }


    #endregion

    #region MISSILE
    private void LaunchMissile()
    {//从中枪口发射导弹
        missile.Launch(muzzleMiddle);
    }
    public void PickUpMissile()
    {//导弹数量+1
        missile.PickUp();
    }
    #endregion

    #region WEAPON POWER

    public void PowerUp()
    {//玩家武器威力提升
        //weaponPower++;
        //weaponPower = Mathf.Clamp(weaponPower, 0, 2);
        TakeDamageNum = 0;//受击次数重置为0
        weaponPower = Mathf.Min(weaponPower + 1, 2);//武器威力+1,限制武器威力最大为2
    }
    void PowerDown()
    {
        weaponPower = Mathf.Max(--weaponPower, 0);
    }

    #endregion

    #region Audio TUNING


    #endregion
}
