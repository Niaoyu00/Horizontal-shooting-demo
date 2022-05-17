using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUi : MonoBehaviour
{
    Text waveText;//�����ı�
    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;//���û���������������
        waveText = GetComponentInChildren<Text>();//��ȡtext���
    }
    private void OnEnable()
    {
        waveText.text = "- WAVE " + EnemyManager.Instance.WaveNumber + " -";
    }
}
