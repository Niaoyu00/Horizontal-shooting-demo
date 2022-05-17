using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : SingLeton<PlayerEnergy>
{
    [SerializeField] EnergyBar energyBar;

    [SerializeField] float overdriveInterval = 0.1f;//�������ʱ�� ����һ������ֵ

    bool isOverdriving = false;//�Ƿ����ڱ���

    public const int MAX = 100;//������� �����ô�д

    public const int PRECENT = 1;//�����ٷֱ� �����ô�д

    private int energy;//��ǰ����

    WaitForSeconds waitForOverdriveInterval;

    protected override void Awake()
    {
        base.Awake();
        waitForOverdriveInterval = new WaitForSeconds(overdriveInterval);
    }
    private void OnEnable()
    {
        PlayerOverdrive.on += PlayerOverdriveOn;
        PlayerOverdrive.off += PlayerOverdriveOff;
    }
    private void OnDisable()
    {
        PlayerOverdrive.on -= PlayerOverdriveOn;
        PlayerOverdrive.off -= PlayerOverdriveOff;
    }
    private void Start()
    {
        energyBar.Initialize(energy, MAX);//��������ʼ��
        Obtain(MAX);//����������
    }
    public void Obtain(int val)
    {//��ȡ��������
        //�����������ʱ �������ڱ���ʱ ���������ȥ ����
        if (energy == MAX || isOverdriving || !gameObject.activeSelf)
            return;
        //energy += val;��ȡ����
        //energy = Mathf.Clamp(energy, 0, MAX);
        energy = Mathf.Clamp(energy + val, 0, MAX);
        energyBar.UpdateStats(energy, MAX);//����������
    }
    public void Use(int val)
    {//��������
        energy -= val;
        energyBar.UpdateStats(energy, MAX);//����������
        if (energy == 0 && isOverdriving)
        {//û����ʱ�رձ���
            PlayerOverdrive.off.Invoke();
        }

    }
    //�ж������Ƿ��㹻
    public bool IsEnough(int val) => energy >= val;
    private void PlayerOverdriveOn()
    {
        isOverdriving = true;
        StartCoroutine(nameof(KeepUsingCoroutine));
    }
    private void PlayerOverdriveOff()
    {
        isOverdriving = false;
        StopCoroutine(nameof(KeepUsingCoroutine));
    }


    IEnumerator KeepUsingCoroutine()
    {//������������
        while (gameObject.activeSelf && energy > 0)
        {
            yield return waitForOverdriveInterval;
            Use(PRECENT);//0.1f s���İٷ�֮һ����
        }
    }
}
