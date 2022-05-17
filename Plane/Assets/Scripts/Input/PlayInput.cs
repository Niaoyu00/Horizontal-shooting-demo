using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
//[CreateAssetMenu]��ScriptableObject�������ͽ��б�ǣ�ʹ���Զ����� Assets/Create �Ӳ˵��У��Ա��ܹ����ɴ��������͵�ʵ����������Ϊ��.asset���ļ��洢����Ŀ�С�
public class PlayInput :
    ScriptableObject,
    PlayerInputActions.IGamePlayActions,
    PlayerInputActions.IPauseMenuActions,
    PlayerInputActions.IGameOverScreenActions
{
    //ScriptableObject:һ���࣬�����Ҫ����[���踽�ӵ���Ϸ����]�Ķ���ʱ���ɴӸ�������������[]��ʾǿ��

    public event UnityAction<Vector2> onMove = delegate { };//��ί����Ϊ��ʼֵ
    public event UnityAction onStopMove = delegate { };//ֹͣ

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
        inputActionsls.GamePlay.SetCallbacks(this);//�Ǽ�gameplay������Ļص����� ������PlayerInputActions��Ľӿ�
        inputActionsls.PauseMenu.SetCallbacks(this);//�Ǽ�PauseMenu������Ļص����� 
        inputActionsls.GameOverScreen.SetCallbacks(this);//�Ǽ�GameOverScreen������Ļص����� 
    }
    private void OnDisable()
    {
        DisableAllInputs();
    }
    void SwitchActionMap(InputActionMap actionMap, bool isUiInput)//�л�������
    {
        inputActionsls.Disable();
        actionMap.Enable();
        if (isUiInput)
        {
            Cursor.visible = true;//��ʾ���
            Cursor.lockState = CursorLockMode.None;//���������
        }
        else
        {
            Cursor.visible = false;//�������
            Cursor.lockState = CursorLockMode.Locked;//�������
        }
    }

    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;//��ģʽ�л��ɶ�̬����
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;//��ģʽ�л��ɹ̶�����(��ͣ��Ϸ���޷�����)
    public void DisableAllInputs()
    {   //��������
        inputActionsls.Disable();
    }
    public void EnableGamePlayInput() => SwitchActionMap(inputActionsls.GamePlay, false);//����GamePlay������,���ع��

    public void EnablePauseMenuInput() => SwitchActionMap(inputActionsls.PauseMenu, true);//������ͣ�˵������,�������
    public void EnableGameOverScreenInput() => SwitchActionMap(inputActionsls.GameOverScreen, false);//�л���Ϸ�����Ķ�����


    public void OnMove(InputAction.CallbackContext context)
    {
        //if (context.phase == InputActionPhase.Performed)//�ƶ������ź�
        if (context.performed)//Disabled:����������ʱ
                              //Waiting�����������ã���û����Ӧ�������źŴ���ʱ
                              //Started�����°�������һ֡,�൱��input.GetKeyDown();
                              //[Performed��]���붯����ִ��(�����˰����Լ���ס���������׶�),�൱��input.GetKey;
                              //Canceled�������ź�ֹͣʱ�������ɿ���������һ֡���൱��input.GetKeyUp();
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
