using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectAudio : MonoBehaviour
{
    Slider slider;
    void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener((float v) =>
        {
            AudioTuning.Instance.SetMasterVolume(v);
        });
    }


}
