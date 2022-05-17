using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIInput : SingLeton<UIInput>
{
    [SerializeField] PlayInput playinput;

    InputSystemUIInputModule uiInputModule;//ui����ģ��
    protected override void Awake()
    {
        base.Awake();
        uiInputModule = GetComponent<InputSystemUIInputModule>();
        uiInputModule.enabled = false;
    }


    public void SelectUI(Selectable UIObject)//ui��ʼ��ѡ�а�ť,Selectable:����ui��Ļ���
    {
        UIObject.Select();//ѡ��ui
        UIObject.OnSelect(null);//��ui���õ���ȷ��״̬
        uiInputModule.enabled = true;
    }
    public void DisableAllUIInputs()
    {//������������
        playinput.DisableAllInputs();
        uiInputModule.enabled = false;
    }
}
