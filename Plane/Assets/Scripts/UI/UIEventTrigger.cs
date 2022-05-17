using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventTrigger :
    MonoBehaviour,
    IPointerEnterHandler,
    IPointerDownHandler,
    ISelectHandler,
    ISubmitHandler
{
    [SerializeField] AudioData selectSFX;
    [SerializeField] AudioData submitSFX;
    public void OnPointerEnter(PointerEventData eventData)
    {//鼠标悬停就会加载此函数
        AudioManager.Instance.PlaySFX(selectSFX);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(submitSFX);

    }
    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX(selectSFX);

    }

    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX(submitSFX);

    }
}
