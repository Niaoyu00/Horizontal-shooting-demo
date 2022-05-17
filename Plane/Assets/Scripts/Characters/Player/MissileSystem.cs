using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] int defauleAmount = 5;//默认导弹数量
    [SerializeField] float cooldownTime = 1f;//冷却时间 默认1s
    [SerializeField] GameObject missilePrefab = null;
    [SerializeField] AudioData launchSFX = null;
    [Header("--------AUDIO TUNING--------")]

    [SerializeField] Slider sliderMissile;//获取sliderui
    bool isReady = true;//导弹是否准备好

    int amount;

    private void Awake()
    {
        amount = defauleAmount;//当前导弹数量（初始化为默认5个）
    }
    private void Start()
    {
        MissileDisplay.UpdateAmountText(amount);//显示现在拥有的导弹数量

        sliderMissile.value = launchSFX.volume;

        StartCoroutine(AudioManager.Instance.AudioTuningCoroutine(launchSFX, sliderMissile));
    }

    public void PickUp()
    {
        amount++;//导弹数量+1
        MissileDisplay.UpdateAmountText(amount);//更新导弹数量ui
        if (amount == 1)
        {
            MissileDisplay.UpdateCooldownImage(0f);//更新图标ui
            isReady = true;
        }
    }

    public void Launch(Transform muzzleTransform)
    {//发射函数
        if (amount == 0 || !isReady)
            return;//禁止发射，直接返回

        isReady = false;//子弹发射，立刻调整为false

        //1，从对象池中取出导弹
        PoolManager.Release(missilePrefab, muzzleTransform.position);
        //2，导弹发射音效
        AudioManager.Instance.PlayRandomSFX(launchSFX);
        amount--;
        MissileDisplay.UpdateAmountText(amount);//显示现在拥有的导弹数量

        if (amount == 0)
        {
            MissileDisplay.UpdateCooldownImage(1f);//使导弹图标变红
        }
        else
        {
            //让导弹进入冷却
            StartCoroutine(CooldownCoroutine());
        }
    }

    IEnumerator CooldownCoroutine()
    {//导弹冷却时间协程 cd为1s
        var cooldownValue = cooldownTime;
        while (cooldownValue > 0)
        {
            MissileDisplay.UpdateCooldownImage(cooldownValue / cooldownTime);//动态更改冷却图标

            cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime, 0f);//Mathf.max()返回参数之间的最大值，此处是为了限制最小不为0；
            yield return null;
        }
        isReady = true;
    }

}
