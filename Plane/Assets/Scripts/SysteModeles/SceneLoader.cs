using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SceneLoader : PersistentSingLeton<SceneLoader>
{
    [SerializeField] Image transitionImage;//转场图片

    [SerializeField] float fadeTime = 3.5f;//淡入出时间

    Color color;

    const string GAMEPLAY = "Gameplay";//场景名

    const string MAINMENU = "MainMenu";//场景名

    const string SCORING = "Scoring";//场景名

    void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    IEnumerator LoadingCoroutine(string sceneName)
    {
        var loadingOperation = SceneManager.LoadSceneAsync(sceneName);//异步加载场景 不然转场景会闪

        loadingOperation.allowSceneActivation = false;//allowSceneActivation 设置加载好的场景是否为激活状态

        transitionImage.gameObject.SetActive(true);//启用转场图片
        //淡出
        while (color.a < 1f)
        {//unscaledDeltaTime不会受到Time.timeScale的影响
            color.a = Mathf.Clamp01(color.a + Time.unscaledDeltaTime / fadeTime);//Clamp01 限制再0-1之间
            transitionImage.color = color;
            yield return null;
        }
        yield return new WaitUntil(() => loadingOperation.progress >= 0.9f);//异步加载场景快好再激活场景 启用成功后为1
        //Load(sceneName);
        loadingOperation.allowSceneActivation = true;//激活场景
        while (color.a > 0f)
        {//unscaledDeltaTime不会受到Time.deltaTime的影响
            color.a = Mathf.Clamp01(color.a - Time.unscaledDeltaTime / fadeTime);//Clamp01 限制再0-1之间
            transitionImage.color = color;
            yield return null;
        }
        transitionImage.gameObject.SetActive(false);


    }

    internal void LoadScoringScene()
    {
        //加载场景前先禁用，防止频繁加载出错
        StopAllCoroutines();
        //加载gameplay场景
        StartCoroutine(LoadingCoroutine(SCORING));
    }

    public void LoadGameplayScene()
    {
        //加载场景前先禁用，防止频繁加载出错
        StopAllCoroutines();
        //加载gameplay场景
        StartCoroutine(LoadingCoroutine(GAMEPLAY));
    }

    public void LoadMainMenuScene()
    {//加载场景前先禁用，防止频繁加载出错
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(MAINMENU));
    }
}
