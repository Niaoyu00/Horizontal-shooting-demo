using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioTuning : SingLeton<AudioTuning>
{
    [SerializeField] AudioMixer audioMixer;
    public void SetMasterVolume(float volume)
    {//vµÈ¼ÛÓÚslider.value
        audioMixer.SetFloat("MasterVolume", volume);

    }

}
