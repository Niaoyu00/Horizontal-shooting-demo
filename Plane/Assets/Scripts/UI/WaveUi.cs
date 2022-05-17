using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUi : MonoBehaviour
{
    Text waveText;//波数文本
    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;//设置画布组件的世界相机
        waveText = GetComponentInChildren<Text>();//获取text组件
    }
    private void OnEnable()
    {
        waveText.text = "- WAVE " + EnemyManager.Instance.WaveNumber + " -";
    }
}
