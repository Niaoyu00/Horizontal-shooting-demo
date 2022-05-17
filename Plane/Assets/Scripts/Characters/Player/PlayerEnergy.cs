using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : SingLeton<PlayerEnergy>
{
    [SerializeField] EnergyBar energyBar;

    [SerializeField] float overdriveInterval = 0.1f;//经过这段时间 消耗一次能量值

    bool isOverdriving = false;//是否正在爆发

    public const int MAX = 100;//最大能量 常量用大写

    public const int PRECENT = 1;//能量百分比 常量用大写

    private int energy;//当前能量

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
        energyBar.Initialize(energy, MAX);//能量条初始化
        Obtain(MAX);//开局满能量
    }
    public void Obtain(int val)
    {//获取能量函数
        //能量等于最大时 或者正在爆发时 或者玩家死去 返回
        if (energy == MAX || isOverdriving || !gameObject.activeSelf)
            return;
        //energy += val;获取能量
        //energy = Mathf.Clamp(energy, 0, MAX);
        energy = Mathf.Clamp(energy + val, 0, MAX);
        energyBar.UpdateStats(energy, MAX);//更新能量条
    }
    public void Use(int val)
    {//能量消耗
        energy -= val;
        energyBar.UpdateStats(energy, MAX);//更新能量条
        if (energy == 0 && isOverdriving)
        {//没能量时关闭爆发
            PlayerOverdrive.off.Invoke();
        }

    }
    //判断能量是否足够
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
    {//不断消耗能量
        while (gameObject.activeSelf && energy > 0)
        {
            yield return waitForOverdriveInterval;
            Use(PRECENT);//0.1f s消耗百分之一能量
        }
    }
}
