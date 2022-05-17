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

    [SerializeField] GameObject triggerVFX;//��Ч
    [SerializeField] GameObject engineVFXNormal;
    [SerializeField] GameObject engineVFXOverdrive;

    [SerializeField] AudioData onSFX;//��Ƶ
    [SerializeField] AudioData offSFX;
    [Header("--------AUDIO TUNING--------")]

    [SerializeField] Slider sliderOverdrive;//����������Ч������
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
    {//�Ӿ���Ч
        triggerVFX.SetActive(true);
        //�����һ�����������Ч��
        engineVFXNormal.SetActive(false);
        engineVFXOverdrive.SetActive(true);
        //���ű�����Ч
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

        //��������������������Э��
        StartCoroutine(AudioManager.Instance.AudioTuningCoroutine(onSFX, sliderOverdrive));
        StartCoroutine(AudioManager.Instance.AudioTuningCoroutine(offSFX, sliderOverdrive));
    }



}
