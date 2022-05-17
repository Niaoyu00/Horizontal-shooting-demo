using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBar : Statsbar_HUD
{
    protected override void SetPercentText()
    {//血条实时目标值转为百分比形式，两位小数
        //percentText.text = (targetFillAmount * 100f).ToString("f2") + "%";
        percentText.text = targetFillAmount.ToString("p");//默认两位小数
    }
}
