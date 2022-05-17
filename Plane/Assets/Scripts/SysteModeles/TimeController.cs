using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : SingLeton<TimeController>
{
    [SerializeField, Range(0f, 1f)] float bulletTimeScale = 0.1f;

    float defaultFixedDeltaTime;

    float timeScaleBeforePause;//��Ϸ��ͣǰ��ʱ��̶�

    float t;
    protected override void Awake()
    {
        base.Awake();
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void Pause()
    {
        timeScaleBeforePause = Time.timeScale;//��ͣʱ��¼�µ�ǰʱ������ (����ʵ����)
        Time.timeScale = 0f;
    }
    public void UnPause()
    {
        Time.timeScale = timeScaleBeforePause;//ȡ����ͣʱ ʹ�ü�¼��ʱ������

    }

    public void BulletTime(float duration)//�����ӵ�ʱ��
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(SlowOutCoroutine(duration));

    }
    public void BulletTime(float inDuration, float OutDuration)//����->�����ӵ�ʱ��
    {
        StartCoroutine(SlowInAndOutCoroutine(inDuration, OutDuration));

    }
    public void BulletTime(float inDuration, float keepingDuration, float OutDuration)//����->ά��->���� �ӵ�ʱ��
    {
        StartCoroutine(SlowInkeepAndOutDuration(inDuration, keepingDuration, OutDuration));
    }


    public void SlowIn(float duration)//�����ӵ�ʱ��
    {
        StartCoroutine(SlowInCoroutine(duration));
    }
    public void SlowOut(float duration)//�����ӵ�
    {
        StartCoroutine(SlowOutCoroutine(duration));
    }

    IEnumerator SlowInkeepAndOutDuration(float inDuration, float keepingDuration, float outDuration)//����->ά��->���� �ӵ�ʱ��Э��
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));

        yield return new WaitForSecondsRealtime(keepingDuration);//�ȴ�����

        StartCoroutine(SlowOutCoroutine(outDuration));
    }

    IEnumerator SlowInAndOutCoroutine(float inDuration, float outDuration)//����->�����ӵ�ʱ�����Э��
    {

        yield return StartCoroutine(SlowInCoroutine(inDuration));

        StartCoroutine(SlowOutCoroutine(outDuration));
    }


    IEnumerator SlowInCoroutine(float duration)//�����ӵ�ʱ��Э��
    {
        t = 0f;
        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)//��Ϸ����ʱ
            {
                // t += Time.deltaTime / duration; unscaledDeltaTime����ʱ��̶�Ӱ��
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;

            }
            yield return null;

        }
    }

    IEnumerator SlowOutCoroutine(float duration)//�����ӵ�ʱ��
    {
        t = 0f;
        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)//��Ϸ����ʱ
            {
                // t += Time.deltaTime / duration; unscaledDeltaTime����ʱ��̶�Ӱ��
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }
            yield return null;
        }
    }
}
