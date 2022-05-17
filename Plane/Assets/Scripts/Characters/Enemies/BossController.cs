using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    [SerializeField] float continuousFireDuration = 1.5f;//�����������ʱ��

    [Header("==== Player Detection ====")]

    [SerializeField] Transform playerDetectionTransfrom;//���еı任���
    [SerializeField] Vector3 playerDetectionSize;//���еĳߴ�
    [SerializeField] LayerMask playerLayer;//��Ҳ�����

    [Header("==== Beam ====")]

    [SerializeField] float beamCooldownTime = 12f;//����������ȴʱ��12s
    [SerializeField] AudioData beamChargingSFX;//����������Ч
    [SerializeField] AudioData beamLaunchSFX;//���ⷢ����Ч

    bool isBeamReady;//���������Ƿ���ȴ

    int launchBeamID = Animator.StringToHash("launchBeam");//ת��Ϊ��ϣ

    WaitForSeconds waitForContinuousFireInterval;//�ȴ���С���ʱ��
    WaitForSeconds waitForFireInterval;//�ȴ�����ʱ��
    WaitForSeconds waitBeamCooldwonTime;//�ȴ�����������ȴʱ��


    List<GameObject> magazine;//�����б� 
    AudioData lauchSFX;//������Ч
    Animator animator;//��ȡ����
    Transform Playertransform;//���λ��(����׷��)

    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();

        waitForContinuousFireInterval = new WaitForSeconds(minFireInterval);//��ʼ���ȴ�ʱ��
        waitForFireInterval = new WaitForSeconds(maxFireInterval);//��ʼ����ȴ�ʱ��
        waitBeamCooldwonTime = new WaitForSeconds(beamCooldownTime);//��ʼ����ȴ�ʱ��

        magazine = new List<GameObject>(projectiles.Length);//Ĭ���������ӵ�����ĳ���

        Playertransform = GameObject.FindGameObjectWithTag("Player").transform;//��ȡ���λ��
    }

    protected override void OnEnable()
    {
        isBeamReady = false;//Ĭ�Ϲرռ���
        muzzleVFX.Stop();//Ĭ�Ϲرտ�����Ч
        StartCoroutine(nameof(BeamCooldownCoroutine));
        base.OnEnable();
    }

    void OnDrawGizmosSelected()
    {//������ⷶΧ
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerDetectionTransfrom.position, playerDetectionSize);
    }

    void ActiveBeamWeapon()
    {//���������
        isBeamReady = false;
        animator.SetTrigger(launchBeamID);//���Ŷ���
        AudioManager.Instance.PlayRandomSFX(beamChargingSFX);//����������Ч
    }
    void AnimationEvent_LaunchBeam()
    {
        AudioManager.Instance.PlayRandomSFX(beamLaunchSFX);//���ŷ�����Ч
    }
    void AnimationEvent_StopBeam()
    {
        StopCoroutine(nameof(ChasingPlayerCoroutine));//ͣ�ø���Э��
        StartCoroutine(nameof(BeamCooldownCoroutine));//����������ȴ
        StartCoroutine(nameof(RandomlyFireCoroutine));//�ٴο����������Э��

    }

    void LoadProjectiles()
    {//�ӵ�װ���

        magazine.Clear();

        //��������boss��ǰ�� (λ�ã��ߴ磬�Ƕȣ��������)
        if (Physics2D.OverlapBox(playerDetectionTransfrom.position, playerDetectionSize, 0f, playerLayer))

        {//�������boss��ǰ�������� �ӵ�1
            magazine.Add(projectiles[0]);//0���ӵ�
            lauchSFX = projectilesLaunchSFX[0];//0����Ч��Ч
        }
        else
        {//����2����3���ӵ� Random.value�����0~1
            if (Random.value < 0.5f)
            {
                magazine.Add(projectiles[1]);//1���ӵ�
                lauchSFX = projectilesLaunchSFX[1];//1���ӵ���Ч
            }
            else
            {
                for (int i = 2; i < projectiles.Length; i++)
                {
                    magazine.Add(projectiles[i]);
                }
                lauchSFX = projectilesLaunchSFX[2];//2���ӵ���Ч
            }
        }
    }

    protected override IEnumerator RandomlyFireCoroutine()
    {//��д�������Э��

        //��Ϸֹͣ���˳�Э��
        while (isActiveAndEnabled)
        {
            if (GameManager.GameState == GameState.GameOver)
                yield break;
            if (isBeamReady)
            {
                ActiveBeamWeapon();//���������
                StartCoroutine(nameof(ChasingPlayerCoroutine));//����׷�����Э��
                yield break;
            }
            yield return waitForFireInterval;//����ȴ������ʱ��
            yield return StartCoroutine(nameof(ContinuousFireCoroutine));//��������Э�� ���ȴ�����ʱ��
        }
    }

    IEnumerator ContinuousFireCoroutine()
    { //����������ǰ,��������Э��

        LoadProjectiles();//�ӵ�װ���
        muzzleVFX.Play();//����ǹ����Ч

        float continuousFireTimer = 0f;//��ʱ
        while (continuousFireTimer < continuousFireDuration)
        {//if Timer<Duration boss���Ͽ���
            foreach (var item in magazine)
            {
                PoolManager.Release(item, muzzle.position);//����ط����ӵ�
            }
            continuousFireTimer += minFireInterval;//���Ӽ��ֵ
            AudioManager.Instance.PlayRandomSFX(lauchSFX);//������Ч����
            yield return waitForContinuousFireInterval;//����ȴ���̵ȴ�ʱ��
        }
        muzzleVFX.Stop();
    }

    IEnumerator BeamCooldownCoroutine()
    {//����������ȴЭ��
        yield return waitBeamCooldwonTime;
        isBeamReady = true;
    }
    IEnumerator ChasingPlayerCoroutine()
    {//������֮��׷�����
        while (isActiveAndEnabled)
        {
            targetPosition.x = ViewPort.Instance.MaxX - enemyPaddingX;//boss x��Ϊ�ӿ����x-bossx
            targetPosition.y = Playertransform.position.y;//y�᲻��
            yield return null;
        }
    }
}
