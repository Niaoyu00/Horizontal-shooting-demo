using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBar : Statsbar_HUD
{
    protected override void SetPercentText()
    {//Ѫ��ʵʱĿ��ֵתΪ�ٷֱ���ʽ����λС��
        //percentText.text = (targetFillAmount * 100f).ToString("f2") + "%";
        percentText.text = targetFillAmount.ToString("p");//Ĭ����λС��
    }
}
