using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIInput : SingLeton<UIInput>
{
    [SerializeField] PlayInput playinput;

    InputSystemUIInputModule uiInputModule;//ui输入模块
    protected override void Awake()
    {
        base.Awake();
        uiInputModule = GetComponent<InputSystemUIInputModule>();
        uiInputModule.enabled = false;
    }


    public void SelectUI(Selectable UIObject)//ui初始化选中按钮,Selectable:所有ui类的基类
    {
        UIObject.Select();//选中ui
        UIObject.OnSelect(null);//将ui设置到正确的状态
        uiInputModule.enabled = true;
    }
    public void DisableAllUIInputs()
    {//禁用所有输入
        playinput.DisableAllInputs();
        uiInputModule.enabled = false;
    }
}
