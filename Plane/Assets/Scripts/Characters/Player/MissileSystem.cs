using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] int defauleAmount = 5;//Ĭ�ϵ�������
    [SerializeField] float cooldownTime = 1f;//��ȴʱ�� Ĭ��1s
    [SerializeField] GameObject missilePrefab = null;
    [SerializeField] AudioData launchSFX = null;
    [Header("--------AUDIO TUNING--------")]

    [SerializeField] Slider sliderMissile;//��ȡsliderui
    bool isReady = true;//�����Ƿ�׼����

    int amount;

    private void Awake()
    {
        amount = defauleAmount;//��ǰ������������ʼ��ΪĬ��5����
    }
    private void Start()
    {
        MissileDisplay.UpdateAmountText(amount);//��ʾ����ӵ�еĵ�������

        sliderMissile.value = launchSFX.volume;

        StartCoroutine(AudioManager.Instance.AudioTuningCoroutine(launchSFX, sliderMissile));
    }

    public void PickUp()
    {
        amount++;//��������+1
        MissileDisplay.UpdateAmountText(amount);//���µ�������ui
        if (amount == 1)
        {
            MissileDisplay.UpdateCooldownImage(0f);//����ͼ��ui
            isReady = true;
        }
    }

    public void Launch(Transform muzzleTransform)
    {//���亯��
        if (amount == 0 || !isReady)
            return;//��ֹ���䣬ֱ�ӷ���

        isReady = false;//�ӵ����䣬���̵���Ϊfalse

        //1���Ӷ������ȡ������
        PoolManager.Release(missilePrefab, muzzleTransform.position);
        //2������������Ч
        AudioManager.Instance.PlayRandomSFX(launchSFX);
        amount--;
        MissileDisplay.UpdateAmountText(amount);//��ʾ����ӵ�еĵ�������

        if (amount == 0)
        {
            MissileDisplay.UpdateCooldownImage(1f);//ʹ����ͼ����
        }
        else
        {
            //�õ���������ȴ
            StartCoroutine(CooldownCoroutine());
        }
    }

    IEnumerator CooldownCoroutine()
    {//������ȴʱ��Э�� cdΪ1s
        var cooldownValue = cooldownTime;
        while (cooldownValue > 0)
        {
            MissileDisplay.UpdateCooldownImage(cooldownValue / cooldownTime);//��̬������ȴͼ��

            cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime, 0f);//Mathf.max()���ز���֮������ֵ���˴���Ϊ��������С��Ϊ0��
            yield return null;
        }
        isReady = true;
    }

}
