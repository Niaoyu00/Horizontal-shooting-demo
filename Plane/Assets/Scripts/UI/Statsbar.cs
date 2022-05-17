using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statsbar : MonoBehaviour
{
    [SerializeField] Image fillImageBack;//获取组件

    [SerializeField] protected Image fillImageFront;

    [SerializeField] bool delayFill = true;//是否需要血条延迟显示

    [SerializeField] float fillSpeed = 0.5f;//第二个血量条的填充速度

    [SerializeField] float fillDelayTime = 0.1f;//血条延迟显示的延迟时间

    float currentFillAmount;//图片的当前填充值

    protected float targetFillAmount;//图片目标填充值

    float previousFillAmount;

    Canvas canvas;

    float t;

    WaitForSeconds waitForDelayFill;

    Coroutine bufferedFillingCoroutine;

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    void Awake()
    {
        if (TryGetComponent<Canvas>(out Canvas canvas))//当能获取画布组件 才将摄像机设为主设
        {
            canvas.worldCamera = Camera.main;
        }

        waitForDelayFill = new WaitForSeconds(fillDelayTime);
    }
    public virtual void Initialize(float currentValue, float maxValue)
    {
        currentFillAmount = currentValue / maxValue;//转化成百分比
        targetFillAmount = currentFillAmount;//延迟展示
        fillImageFront.fillAmount = currentFillAmount;//ui中显示
        fillImageBack.fillAmount = currentFillAmount;//ui中显示
    }
    public void UpdateStats(float currentValue, float maxValue)
    {
        targetFillAmount = currentValue / maxValue;//转化成百分比
        if (bufferedFillingCoroutine != null)
        {//停用带参数的协程，防止多次触发
            StopCoroutine(bufferedFillingCoroutine);
        }
        //状态更新：if扣血时
        if (currentFillAmount > targetFillAmount)//当前值大于目标值（说明扣血了）
        {
            //front（前面的图片）=target;立刻更新为目标值
            fillImageFront.fillAmount = targetFillAmount;
            //back （后面的图片）进度条慢慢减少的效果（延迟动画）
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));
        }
        else if (currentFillAmount < targetFillAmount)
        {//状态更新：if回血时
         //back （后面的图片）=target;立刻更新为目标值
            fillImageBack.fillAmount = targetFillAmount;
            //front（前面的图片）进度条慢慢增加（延迟动画）
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageFront));

        }

    }
    protected virtual IEnumerator BufferedFillingCoroutine(Image img)
    {

        //缓慢填充协程
        if (delayFill)
        {
            yield return waitForDelayFill;
        }
        previousFillAmount = currentFillAmount;
        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * fillSpeed;

            currentFillAmount = Mathf.Lerp(previousFillAmount, targetFillAmount, t);

            img.fillAmount = currentFillAmount;

            yield return null;
        }


    }
}
