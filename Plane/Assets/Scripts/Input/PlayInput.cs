using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
//[CreateAssetMenu]对ScriptableObject派生类型进行标记，使其自动列在 Assets/Create 子菜单中，以便能够轻松创建该类型的实例并将其作为“.asset”文件存储在项目中。
public class PlayInput :
    ScriptableObject,
    PlayerInputActions.IGamePlayActions,
    PlayerInputActions.IPauseMenuActions,
    PlayerInputActions.IGameOverScreenActions
{
    //ScriptableObject:一个类，如果需要创建[无需附加到游戏对象]的对象时，可从该类派生。这里[]表示强调

    public event UnityAction<Vector2> onMove = delegate { };//空委托作为初始值
    public event UnityAction onStopMove = delegate { };//停止

    public event UnityAction onFire = delegate { };
    public event UnityAction onStopFire = delegate { };

    public event UnityAction onDodge = delegate { };

    public event UnityAction onOverdrive = delegate { };

    public event UnityAction onPause = delegate { };
    public event UnityAction onUnpause = delegate { };

    public event UnityAction onLaunchMissile = delegate { };

    public event UnityAction onConfirmGameOver = delegate { };


    PlayerInputActions inputActionsls;
    private void OnEnable()
    {
        inputActionsls = new PlayerInputActions();
        inputActionsls.GamePlay.SetCallbacks(this);//登记gameplay动作表的回调函数 参数：PlayerInputActions类的接口
        inputActionsls.PauseMenu.SetCallbacks(this);//登记PauseMenu动作表的回调函数 
        inputActionsls.GameOverScreen.SetCallbacks(this);//登记GameOverScreen动作表的回调函数 
    }
    private void OnDisable()
    {
        DisableAllInputs();
    }
    void SwitchActionMap(InputActionMap actionMap, bool isUiInput)//切换动作表
    {
        inputActionsls.Disable();
        actionMap.Enable();
        if (isUiInput)
        {
            Cursor.visible = true;//显示鼠标
            Cursor.lockState = CursorLockMode.None;//不锁定光标
        }
        else
        {
            Cursor.visible = false;//隐藏鼠标
            Cursor.lockState = CursorLockMode.Locked;//鼠标锁定
        }
    }

    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;//将模式切换成动态更新
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;//将模式切换成固定更新(暂停游戏后无法操作)
    public void DisableAllInputs()
    {   //禁用输入
        inputActionsls.Disable();
    }
    public void EnableGamePlayInput() => SwitchActionMap(inputActionsls.GamePlay, false);//启用GamePlay动作表,隐藏光标

    public void EnablePauseMenuInput() => SwitchActionMap(inputActionsls.PauseMenu, true);//启用暂停菜单输入表,开启光标
    public void EnableGameOverScreenInput() => SwitchActionMap(inputActionsls.GameOverScreen, false);//切换游戏结束的动作表


    public void OnMove(InputAction.CallbackContext context)
    {
        //if (context.phase == InputActionPhase.Performed)//移动输入信号
        if (context.performed)//Disabled:动作表被禁用时
                              //Waiting：动作表被启用，但没有相应的输入信号传入时
                              //Started：按下按键的那一帧,相当于input.GetKeyDown();
                              //[Performed：]输入动作已执行(包含了按下以及按住按键两个阶段),相当于input.GetKey;
                              //Canceled：输入信号停止时，就是松开按键的那一帧。相当于input.GetKeyUp();
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }
        //if (context.phase == InputActionPhase.Canceled)
        if (context.canceled)
        {
            onStopMove.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onFire.Invoke();
        }
        if (context.canceled)
        {
            onStopFire.Invoke();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onDodge.Invoke();
        }
    }

    public void OnOverdrive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onOverdrive.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onPause.Invoke();
        }
    }

    public void OnUnpause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onUnpause.Invoke();
        }
    }

    public void OnLaunchMissile(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onLaunchMissile.Invoke();
        }
    }

    public void OnConfirmGameOver(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onConfirmGameOver.Invoke();
        }
    }
}
