using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : SingLeton<TimeController>
{
    [SerializeField, Range(0f, 1f)] float bulletTimeScale = 0.1f;

    float defaultFixedDeltaTime;

    float timeScaleBeforePause;//游戏暂停前的时间刻度

    float t;
    protected override void Awake()
    {
        base.Awake();
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void Pause()
    {
        timeScaleBeforePause = Time.timeScale;//暂停时记录下当前时间流速 (用来实现在)
        Time.timeScale = 0f;
    }
    public void UnPause()
    {
        Time.timeScale = timeScaleBeforePause;//取消暂停时 使用记录的时间流速

    }

    public void BulletTime(float duration)//开启子弹时间
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(SlowOutCoroutine(duration));

    }
    public void BulletTime(float inDuration, float OutDuration)//开启->结束子弹时间
    {
        StartCoroutine(SlowInAndOutCoroutine(inDuration, OutDuration));

    }
    public void BulletTime(float inDuration, float keepingDuration, float OutDuration)//开启->维持->结束 子弹时间
    {
        StartCoroutine(SlowInkeepAndOutDuration(inDuration, keepingDuration, OutDuration));
    }


    public void SlowIn(float duration)//开启子弹时间
    {
        StartCoroutine(SlowInCoroutine(duration));
    }
    public void SlowOut(float duration)//结束子弹
    {
        StartCoroutine(SlowOutCoroutine(duration));
    }

    IEnumerator SlowInkeepAndOutDuration(float inDuration, float keepingDuration, float outDuration)//开启->维持->结束 子弹时间协程
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));

        yield return new WaitForSecondsRealtime(keepingDuration);//等待几秒

        StartCoroutine(SlowOutCoroutine(outDuration));
    }

    IEnumerator SlowInAndOutCoroutine(float inDuration, float outDuration)//开启->结束子弹时间过程协程
    {

        yield return StartCoroutine(SlowInCoroutine(inDuration));

        StartCoroutine(SlowOutCoroutine(outDuration));
    }


    IEnumerator SlowInCoroutine(float duration)//开启子弹时间协程
    {
        t = 0f;
        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)//游戏运行时
            {
                // t += Time.deltaTime / duration; unscaledDeltaTime不受时间刻度影响
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;

            }
            yield return null;

        }
    }

    IEnumerator SlowOutCoroutine(float duration)//结束子弹时间
    {
        t = 0f;
        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)//游戏运行时
            {
                // t += Time.deltaTime / duration; unscaledDeltaTime不受时间刻度影响
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }
            yield return null;
        }
    }
}
