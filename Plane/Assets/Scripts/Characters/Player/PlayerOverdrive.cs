using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerOverdrive : MonoBehaviour
{
    public static UnityAction on = delegate { };
    public static UnityAction off = delegate { };

    [SerializeField] GameObject triggerVFX;//特效
    [SerializeField] GameObject engineVFXNormal;
    [SerializeField] GameObject engineVFXOverdrive;

    [SerializeField] AudioData onSFX;//音频
    [SerializeField] AudioData offSFX;
    [Header("--------AUDIO TUNING--------")]

    [SerializeField] Slider sliderOverdrive;//能量爆发音效调整条
    private void Awake()
    {
        on += On;
        off += Off;
    }
    private void OnDestroy()
    {
        on -= On;
        off -= Off;
        StopAllCoroutines();
    }
    private void On()
    {//视觉特效
        triggerVFX.SetActive(true);
        //变更玩家机体引擎粒子效果
        engineVFXNormal.SetActive(false);
        engineVFXOverdrive.SetActive(true);
        //播放爆发音效
        AudioManager.Instance.PlayRandomSFX(onSFX);
    }
    private void Off()
    {
        engineVFXOverdrive.SetActive(false);
        engineVFXNormal.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(offSFX);
    }

    void Start()
    {

        //启动能量爆发音量调整协程
        StartCoroutine(AudioManager.Instance.AudioTuningCoroutine(onSFX, sliderOverdrive));
        StartCoroutine(AudioManager.Instance.AudioTuningCoroutine(offSFX, sliderOverdrive));
    }



}
