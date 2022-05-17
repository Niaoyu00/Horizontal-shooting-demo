using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statsbar_HUD : Statsbar
{
    [SerializeField] protected Text percentText;

    protected virtual void SetPercentText()
    {//设置百分比文本
     //Mathf.RoundToInt:将浮点数转化成最近的整数,targetFillAmount:血条目标值
     //percentText.text = Mathf.RoundToInt(targetFillAmount * 100f) + "%";
        percentText.text = targetFillAmount.ToString("P0");//显示成没有小数的百分比值
    }
    public override void Initialize(float currentValue, float maxValue)
    {
        base.Initialize(currentValue, maxValue);
        SetPercentText();
    }
    protected override IEnumerator BufferedFillingCoroutine(Image img)
    {
        SetPercentText();
        return base.BufferedFillingCoroutine(img);
    }
}
