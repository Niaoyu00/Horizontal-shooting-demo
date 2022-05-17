using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]//��Ӹ������
public class Player : Character
{
    #region FIELDS

    [SerializeField] Statsbar_HUD statsbar_HUD;

    [SerializeField] bool regenerateHealth = true;//�Ƿ�����

    [SerializeField] float HealthregenerateTime;//����ʱ��

    [SerializeField, Range(0f, 1f)] float HealthregenerateTimePercent;//�����ٷֱ�

    [SerializeField] GameObject invincibleVFX;//�޵���Ч

    [Header("--------INPUT--------")]
    [SerializeField] PlayInput input;

    [Header("--------MOVE--------")]
    [SerializeField] private float moveSpeed = 6f;

    [SerializeField] float accelerationTime = 3f;//����ʱ��

    [SerializeField] float decelerationTime = 3f;//����ʱ��

    [SerializeField] float moveRotationAngle = 50f;//�ƶ���ת�Ƕ�

    [Header("--------FIRE--------")]
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject projectile1;
    [SerializeField] GameObject projectile2;
    [SerializeField] GameObject projectileOverdrive;

    [SerializeField] ParticleSystem muzzleVFX;//ǹ����Ч

    [SerializeField] Transform muzzleTop;//��ǹ��
    [SerializeField] Transform muzzleMiddle;//��ǹ��
    [SerializeField] Transform muzzleBottom;//��ǹ��

    [SerializeField] AudioData projectileSFX;//�ӵ�������Ч

    [SerializeField, Range(0, 2)] int weaponPower = 0;//�������� Range()ȡֵ��Χ

    [SerializeField] float fireInterval = 0.2f;

    [Header("--------DODGE--------")]

    [SerializeField] AudioData dodgeSFX;//��ȡ������Ч

    [SerializeField, Range(0, 100)] int dodgeEnergyCost = 25;//������������ֵ

    [SerializeField] float maxRoll = 720;//����ת�Ƕ�

    [SerializeField] float rollSpeed = 360;//����ת�Ƕ�

    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);//����ʱ��С����ֵ

    [Header("--------OVERDRIVE--------")]

    [SerializeField] int overdriveDodgeFactor = 2;

    [SerializeField] float overdriveSpeedFactor = 1.2f;

    [SerializeField] float overdriveFireFactor = 1.2f;

    [Header("--------AUDIO TUNING--------")]

    [SerializeField] Slider sliderDodge;//��ȡ������Ч����ui 
    [SerializeField] Slider sliderProjectile;//��ȡ�ӵ�������Ч����ui
    [SerializeField] Slider sliderDie;//��ȡ���������Ч����ui



    bool isOverdriving = false;//�Ƿ�������

    bool isDodging = false;//������

    float dodgeDuration;//���ܳ���ʱ��

    float currentRoll;//��ǰ��ת�Ƕ�

    float paddingX;//��Ҫƫ�Ƶ���(ֵΪģ��x���ȵ�һ��)

    float paddingY;//��Ҫƫ�Ƶ���(ֵΪģ��y���ȵ�һ��)

    float t;

    int TakeDamageNum = 0;//�ܻ�����

    readonly float slowMotionDuration = 1f;//��ʼ���ӵ�ʱ�� ����ʱ�� 

    [SerializeField] readonly float InvincibleTime = 1f;//��ɫ���� �޵г���ʱ��

    WaitForSeconds waitForOverdriveFireInterval;//�ȴ���������ʱ�Ŀ�����

    WaitForSeconds waitForFireInterval;//�ȴ�������

    WaitForSeconds waitHealthRegenerateTime;//�ȴ�����ֵ������ʱ��

    WaitForSeconds waitInvincibleTime;//�޵�ʱ��

    Coroutine moveC;//������ֵ������ͣ�ô�������Э��

    Coroutine HealthRegenerateC;//������ֵ������ͣ�ô�������Э��

    new Rigidbody2D rigidbody;//��ȡ����

    new Collider2D collider;//��ȡ��ײ

    Vector2 previousVelocity;
    Vector2 moveDirection;//�ƶ�����
    Quaternion previousRotation;

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    MissileSystem missile;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        missile = GetComponent<MissileSystem>();

        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;//���ģ�ͳߴ� 
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;

        dodgeDuration = maxRoll / rollSpeed;// �����ת��/�����ٶ�=����ʱ��
        rigidbody.gravityScale = 0f;//������Ϊ0


        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval /= overdriveFireFactor);
        waitHealthRegenerateTime = new WaitForSeconds(HealthregenerateTime);
        waitInvincibleTime = new WaitForSeconds(InvincibleTime);

        TakeDamageNum = 0;//�ܻ�����Ĭ��Ϊ0
    }
    void Start()
    {
        statsbar_HUD.Initialize(health, maxHealth);
        input.EnableGamePlayInput();//�������

        StartCoroutine(AudioManager.Instance.AudioTuningCoroutine(dodgeSFX, sliderDodge));//����������������Э��[��Ч����,ui����������]
        StartCoroutine(AudioManager.Instance.AudioTuningCoroutine(projectileSFX, sliderProjectile));//�����ӵ�������������Э��
        StartCoroutine(AudioManager.Instance.AudioTuningCoroutine(deathSFX, sliderDie));//���������Ч����


    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    private void Update()
    {
        StartCoroutine(nameof(MoveRangeLimatationCoroutine));//����λ������Я��(�������ӿ���)

    }
    protected override void OnEnable()
    {
        base.OnEnable();
        //�����¼�
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
    {//ɾ���¼�
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

    public bool IsFullHealth => health == maxHealth;//��ǰѪ���Ƿ�Ϊ��
    public bool IsFUllPower => weaponPower == 2;//�ж����������Ƿ�Ϊ2


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
        {//������3�κ󽵵������ȼ�
            PowerDown();//���������ȼ�
            TakeDamageNum = 0;
        }

        statsbar_HUD.UpdateStats(health, maxHealth);//huduiѪ������
        TimeController.Instance.BulletTime(0.5f);//�ӵ�ʱ��

        if (gameObject.activeSelf)
        {
            Move(moveDirection);
            StartCoroutine(InvincibleCoroutine());//�޵�״̬ 1s
            if (regenerateHealth)//�����Ѫ���ؿ���
            {
                if (HealthRegenerateC != null)
                {//�����Ϊ�գ���ͣ��
                    StopCoroutine(HealthRegenerateC);
                }//�����Ի�ѪЭ��
                HealthRegenerateC = StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenerateTime, HealthregenerateTimePercent));
            }
        }
    }
    public override void Die()
    {
        GameManager.onGameOver?.Invoke();
        GameManager.GameState = GameState.GameOver;//��Ϸ״̬����
        statsbar_HUD.UpdateStats(0f, maxHealth);
        base.Die();
    }
    #endregion

    #region FIRE
    private void Fire()
    {
        muzzleVFX.Play();//����ǹ����Ч
        //StartCoroutine("FireCoroutine");
        StartCoroutine(nameof(FireCoroutine));//��ȡ���� �ַ�����ʽ

    }
    private void StopFire()
    {
        muzzleVFX.Stop();//�ر�ǹ����Ч
        //StopCoroutine("FireCoroutine");
        StopCoroutine(nameof(FireCoroutine));
    }
    IEnumerator FireCoroutine()
    {

        while (true)
        {   //��instantiate���������ӵ�
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
            {//����������ӵ�
                case 0://����֮ǰ�ж��Ƿ��Ǳ���״̬�������򷵻ر����ӵ� ������þɵ�
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
            AudioManager.Instance.PlayRandomSFX(projectileSFX);//��Ч����
            //�ж��Ƿ񱬷��У��ǵĻ�ִ�б���������(����)����ִ��ԭ��������������
            yield return isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;

        }
    }

    IEnumerator InvincibleCoroutine()
    {//�����޵�1s
        collider.isTrigger = true;//��ײ�忪��
        invincibleVFX.SetActive(true);//�޵���Ч����
        yield return waitInvincibleTime;
        collider.isTrigger = false;
        invincibleVFX.SetActive(false);
    }
    IEnumerator ShieldCoroutine(float s)
    {//��ȡ����,�޵�5s
        collider.isTrigger = true;//��ײ�忪��
        invincibleVFX.SetActive(true);
        yield return new WaitForSeconds(s);
        collider.isTrigger = false;
        invincibleVFX.SetActive(false);

    }
    public void PickShield(float s)
    {//��ȡ����ʱ �޵�s��
        StartCoroutine(ShieldCoroutine(s));
    }

    #endregion

    #region MOVE
    private void Move(Vector2 moveInput)
    {
        //��ֹ֮ͣǰ�ļ���Я�������С�
        if (moveC != null)
        {
            StopCoroutine(moveC);
        }
        //�ض����ϵ���ת�Ƕ� moveInput.y ����[-1.1]
        Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);

        moveDirection = moveInput.normalized;
        //�����źŶ�ά������ֵ*�ٶȣ��õ��ƶ���
        //rigidbody.velocity = moveInput * moveSpeed; //������ٶ�ֵ
        moveC = StartCoroutine(MoveCoroutine(accelerationTime, moveDirection * moveSpeed, moveRotation));//��������


    }
    private void StopMove()
    {
        //��ֹ֮ͣǰ�ļ���Я�������С�
        if (moveC != null)
        {
            StopCoroutine(moveC);
        }
        moveDirection = Vector2.zero;
        //rigidbody.velocity = Vector2.zero;
        moveC = StartCoroutine(MoveCoroutine(decelerationTime, moveDirection, Quaternion.identity));//����
        StopCoroutine(nameof(MoveRangeLimatationCoroutine));//ֹͣλ������Я��
    }

    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation)
    {//���ټ���
        t = 0f;
        previousVelocity = rigidbody.velocity;//rigidbody.velocity������ı䣬�����ȴ洢����������
        previousRotation = transform.rotation;//������ı䣬�����ȴ洢����������
        //while (t < 1f)
        //{
        //    t += Time.fixedDeltaTime / time;
        //    rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, moveVelocity, t );//�ƶ� t / time��һ��0~1��ֵ
        //    transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, t );//��ת
        //    yield return null;
        //}
        while (t < time)
        {
            t += Time.fixedDeltaTime;
            rigidbody.velocity = Vector2.Lerp(previousVelocity, moveVelocity, t / time);//�ƶ� t / time��һ��0~1��ֵ
            transform.rotation = Quaternion.Lerp(previousRotation, moveRotation, t / time);//��ת
            yield return waitForFixedUpdate;//����ȴ���һ�θ���
        }
    }
    IEnumerator MoveRangeLimatationCoroutine()
    {
        //λ������Я��
        while (true)
        {
            //������Ҳ������ӿ�
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
        isDodging = true;//���ڷ���
        currentRoll = 0f;//��ǰ��ת�Ƕȹ���
        AudioManager.Instance.PlayRandomSFX(dodgeSFX);
        //����ֵ���� 25
        PlayerEnergy.Instance.Use(dodgeEnergyCost);
        //����ʱ����޵�
        collider.isTrigger = true;
        var scale = transform.localScale;
        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);//�ӵ�ʱ��
        //����ʱ�����x����ת
        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;//��ת
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);

            if (currentRoll < maxRoll / 2f)
            {//��ת��һ��ʱ���ɻ�����
                scale -= (Time.deltaTime / dodgeDuration) * Vector3.one;
                //Debug.Log("��С��" + scale);
            }
            else
            {
                scale += (Time.deltaTime / dodgeDuration) * Vector3.one;
                //��Ϊ�����ƶ�������ת���жϲ��ԣ����¸�ԭ��Ⱦɵķɻ�������˼�һ���ж�ǿ�ƻ�ԭ��1����
                if (scale.x > 1 && scale.y > 1 && scale.z > 1)
                {
                    scale = Vector3.one;
                }
                //Debug.Log("��ԭ��" + scale);

            }
            transform.localScale = scale;
            yield return null;
        }
        //�������ģ���Դﵽ��Զ�����е�Ч��
        collider.isTrigger = false;
        isDodging = false;
    }

    #endregion

    #region OVERDRIVE
    private void Overdrive()
    {//����ֵ����ʱ ����
        if (!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX))
            return;
        else
        {
            PlayerOverdrive.on.Invoke();//������������
        }
    }
    private void OverdriveOn()
    {//������������
        isOverdriving = true;
        dodgeEnergyCost *= overdriveDodgeFactor;//����(����������)����ֵ�����������ӣ�����ϵ����
        moveSpeed *= overdriveSpeedFactor;//�������ӣ�����ϵ����
        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);//�ӵ�ʱ��
    }
    private void OverdriveOff()
    {//�ر���������
        isOverdriving = false;
        dodgeEnergyCost /= overdriveDodgeFactor;//�ָ�����
        moveSpeed /= overdriveSpeedFactor;//�ָ�����
    }


    #endregion

    #region MISSILE
    private void LaunchMissile()
    {//����ǹ�ڷ��䵼��
        missile.Launch(muzzleMiddle);
    }
    public void PickUpMissile()
    {//��������+1
        missile.PickUp();
    }
    #endregion

    #region WEAPON POWER

    public void PowerUp()
    {//���������������
        //weaponPower++;
        //weaponPower = Mathf.Clamp(weaponPower, 0, 2);
        TakeDamageNum = 0;//�ܻ���������Ϊ0
        weaponPower = Mathf.Min(weaponPower + 1, 2);//��������+1,���������������Ϊ2
    }
    void PowerDown()
    {
        weaponPower = Mathf.Max(--weaponPower, 0);
    }

    #endregion

    #region Audio TUNING


    #endregion
}
