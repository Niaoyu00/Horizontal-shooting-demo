using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : PersistentSingLeton<AudioManager>
{
    [SerializeField] AudioSource sFXPlayer;//音效播放器
    const float MIN_PITH = 0.9f;//最小音高
    const float MAX_PUTH = 1.1f;//最大音高

    //PlaySFX适合播放ui音效
    public void PlaySFX(AudioData audioData)//volume 音量 ；andioClip 音频剪辑
    {
        //sFXPlayer.clip = andioClip;音频剪辑的参数赋值到音效播放器
        //sFXPlayer.volume = volume;音量
        //sFXPlayer.Play();播放,重复调用会打断
        sFXPlayer.PlayOneShot(audioData.audioClip, audioData.volume);//PlayOneShot播放,重复调用不会覆盖，不需要改变音源组件
    }
    //PlayRandomSFX适合播放子弹类重复音效
    public void PlayRandomSFX(AudioData audioData)
    {
        sFXPlayer.pitch = Random.Range(MIN_PITH, MAX_PUTH);//随机音高
        PlaySFX(audioData);
    }
    public void PlayRandomSFX(AudioData[] audioDatas)
    {
        PlayRandomSFX(audioDatas[Random.Range(0, audioDatas.Length)]);//数组中随机选一个作为音效
    }
    public virtual IEnumerator AudioTuningCoroutine(AudioData audioData, Slider slider)
    {//音量调整协程
        while (true)
        {
            audioData.volume = slider.value;//ui条的内容赋值给音量
            yield return null;
        }
    }
}
