using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statsbar : MonoBehaviour
{
    [SerializeField] Image fillImageBack;//��ȡ���

    [SerializeField] protected Image fillImageFront;

    [SerializeField] bool delayFill = true;//�Ƿ���ҪѪ���ӳ���ʾ

    [SerializeField] float fillSpeed = 0.5f;//�ڶ���Ѫ����������ٶ�

    [SerializeField] float fillDelayTime = 0.1f;//Ѫ���ӳ���ʾ���ӳ�ʱ��

    float currentFillAmount;//ͼƬ�ĵ�ǰ���ֵ

    protected float targetFillAmount;//ͼƬĿ�����ֵ

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
        if (TryGetComponent<Canvas>(out Canvas canvas))//���ܻ�ȡ������� �Ž��������Ϊ����
        {
            canvas.worldCamera = Camera.main;
        }

        waitForDelayFill = new WaitForSeconds(fillDelayTime);
    }
    public virtual void Initialize(float currentValue, float maxValue)
    {
        currentFillAmount = currentValue / maxValue;//ת���ɰٷֱ�
        targetFillAmount = currentFillAmount;//�ӳ�չʾ
        fillImageFront.fillAmount = currentFillAmount;//ui����ʾ
        fillImageBack.fillAmount = currentFillAmount;//ui����ʾ
    }
    public void UpdateStats(float currentValue, float maxValue)
    {
        targetFillAmount = currentValue / maxValue;//ת���ɰٷֱ�
        if (bufferedFillingCoroutine != null)
        {//ͣ�ô�������Э�̣���ֹ��δ���
            StopCoroutine(bufferedFillingCoroutine);
        }
        //״̬���£�if��Ѫʱ
        if (currentFillAmount > targetFillAmount)//��ǰֵ����Ŀ��ֵ��˵����Ѫ�ˣ�
        {
            //front��ǰ���ͼƬ��=target;���̸���ΪĿ��ֵ
            fillImageFront.fillAmount = targetFillAmount;
            //back �������ͼƬ���������������ٵ�Ч�����ӳٶ�����
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));
        }
        else if (currentFillAmount < targetFillAmount)
        {//״̬���£�if��Ѫʱ
         //back �������ͼƬ��=target;���̸���ΪĿ��ֵ
            fillImageBack.fillAmount = targetFillAmount;
            //front��ǰ���ͼƬ���������������ӣ��ӳٶ�����
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageFront));

        }

    }
    protected virtual IEnumerator BufferedFillingCoroutine(Image img)
    {

        //�������Э��
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
