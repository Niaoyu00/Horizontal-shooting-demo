using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statsbar_HUD : Statsbar
{
    [SerializeField] protected Text percentText;

    protected virtual void SetPercentText()
    {//���ðٷֱ��ı�
     //Mathf.RoundToInt:��������ת�������������,targetFillAmount:Ѫ��Ŀ��ֵ
     //percentText.text = Mathf.RoundToInt(targetFillAmount * 100f) + "%";
        percentText.text = targetFillAmount.ToString("P0");//��ʾ��û��С���İٷֱ�ֵ
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
