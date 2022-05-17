using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SelectBGM : MonoBehaviour
{
    Slider slider;
    AudioSource audioSource;
    void Awake()
    {
        slider = GetComponent<Slider>();
        audioSource = GameObject.FindWithTag("MusicPlayer").GetComponent<AudioSource>();

        slider.onValueChanged.AddListener((float v) =>
        {
            audioSource.volume = v;
        });
    }
    void Start()
    {
    }

}
