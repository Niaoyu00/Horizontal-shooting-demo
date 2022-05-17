using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SceneLoader : PersistentSingLeton<SceneLoader>
{
    [SerializeField] Image transitionImage;//ת��ͼƬ

    [SerializeField] float fadeTime = 3.5f;//�����ʱ��

    Color color;

    const string GAMEPLAY = "Gameplay";//������

    const string MAINMENU = "MainMenu";//������

    const string SCORING = "Scoring";//������

    void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    IEnumerator LoadingCoroutine(string sceneName)
    {
        var loadingOperation = SceneManager.LoadSceneAsync(sceneName);//�첽���س��� ��Ȼת��������

        loadingOperation.allowSceneActivation = false;//allowSceneActivation ���ü��غõĳ����Ƿ�Ϊ����״̬

        transitionImage.gameObject.SetActive(true);//����ת��ͼƬ
        //����
        while (color.a < 1f)
        {//unscaledDeltaTime�����ܵ�Time.timeScale��Ӱ��
            color.a = Mathf.Clamp01(color.a + Time.unscaledDeltaTime / fadeTime);//Clamp01 ������0-1֮��
            transitionImage.color = color;
            yield return null;
        }
        yield return new WaitUntil(() => loadingOperation.progress >= 0.9f);//�첽���س�������ټ���� ���óɹ���Ϊ1
        //Load(sceneName);
        loadingOperation.allowSceneActivation = true;//�����
        while (color.a > 0f)
        {//unscaledDeltaTime�����ܵ�Time.deltaTime��Ӱ��
            color.a = Mathf.Clamp01(color.a - Time.unscaledDeltaTime / fadeTime);//Clamp01 ������0-1֮��
            transitionImage.color = color;
            yield return null;
        }
        transitionImage.gameObject.SetActive(false);


    }

    internal void LoadScoringScene()
    {
        //���س���ǰ�Ƚ��ã���ֹƵ�����س���
        StopAllCoroutines();
        //����gameplay����
        StartCoroutine(LoadingCoroutine(SCORING));
    }

    public void LoadGameplayScene()
    {
        //���س���ǰ�Ƚ��ã���ֹƵ�����س���
        StopAllCoroutines();
        //����gameplay����
        StartCoroutine(LoadingCoroutine(GAMEPLAY));
    }

    public void LoadMainMenuScene()
    {//���س���ǰ�Ƚ��ã���ֹƵ�����س���
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(MAINMENU));
    }
}
