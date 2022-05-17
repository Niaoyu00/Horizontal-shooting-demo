using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : PersistentSingLeton<AudioManager>
{
    [SerializeField] AudioSource sFXPlayer;//��Ч������
    const float MIN_PITH = 0.9f;//��С����
    const float MAX_PUTH = 1.1f;//�������

    //PlaySFX�ʺϲ���ui��Ч
    public void PlaySFX(AudioData audioData)//volume ���� ��andioClip ��Ƶ����
    {
        //sFXPlayer.clip = andioClip;��Ƶ�����Ĳ�����ֵ����Ч������
        //sFXPlayer.volume = volume;����
        //sFXPlayer.Play();����,�ظ����û���
        sFXPlayer.PlayOneShot(audioData.audioClip, audioData.volume);//PlayOneShot����,�ظ����ò��Ḳ�ǣ�����Ҫ�ı���Դ���
    }
    //PlayRandomSFX�ʺϲ����ӵ����ظ���Ч
    public void PlayRandomSFX(AudioData audioData)
    {
        sFXPlayer.pitch = Random.Range(MIN_PITH, MAX_PUTH);//�������
        PlaySFX(audioData);
    }
    public void PlayRandomSFX(AudioData[] audioDatas)
    {
        PlayRandomSFX(audioDatas[Random.Range(0, audioDatas.Length)]);//���������ѡһ����Ϊ��Ч
    }
    public virtual IEnumerator AudioTuningCoroutine(AudioData audioData, Slider slider)
    {//��������Э��
        while (true)
        {
            audioData.volume = slider.value;//ui�������ݸ�ֵ������
            yield return null;
        }
    }
}
