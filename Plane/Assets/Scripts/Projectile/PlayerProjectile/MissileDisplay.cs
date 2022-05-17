using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileDisplay : MonoBehaviour
{
    static Text amountText;
    static Image cooldownImage;
    private void Awake()
    {
        amountText = transform.Find("AmountText").GetComponent<Text>();
        cooldownImage = transform.Find("CooldowmImage").GetComponent<Image>();

    }
    public static void UpdateCooldownImage(float fillamount)
    {//������ȴͼƬ
        cooldownImage.fillAmount = fillamount;
    }
    //�����ı�
    public static void UpdateAmountText(int amount) => amountText.text = amount.ToString();
}
