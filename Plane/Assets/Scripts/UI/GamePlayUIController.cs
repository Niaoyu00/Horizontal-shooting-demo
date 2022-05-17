using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUIController : MonoBehaviour
{
    [Header("===== PLAYER INPUT =====")]

    [SerializeField] PlayInput playInput;

    [Header("===== AUDIO DATA =====")]
    [SerializeField] AudioData pauseSFX;//��Ч

    [SerializeField] AudioData unpauseSFX;

    [Header("===== CANVAS =====")]

    [SerializeField] Canvas hUDCanvas;

    [SerializeField] Canvas menusCanvas;

    //[SerializeField] Canvas AudioTuning;//������

    // [SerializeField] Canvas PauseMenu;//��ͣ�˵�

    [Header("===== BUTTON =====")]

    [SerializeField] Button btnResume;//ͨ��ui���¼��󶨹�

    [SerializeField] Button btnOptions;

    [SerializeField] Button btnMainMenu;

    [SerializeField] Button btnBack;

    [Header("===== GameObject =====")]

    [SerializeField] GameObject AudioTuning;
    [SerializeField] GameObject PauseMenu;


    int buttonPressedParameterID = Animator.StringToHash("Pressed");//ת��Ϊ��ϣֵ

    private void OnEnable()
    {
        playInput.onPause += Pause;
        playInput.onUnpause += Unpause;

        //btnMainMenu.onClick.AddListener(OnResumeButtonClick);
        //btnMainMenu.onClick.AddListener(OnMainMenuButtonClick);
        // btnOptions.onClick.AddListener(OnOptionsButtonClick);

        ButtonPressed.buttonFunctionTable.Add(btnResume.gameObject.name, OnResumeButtonClick);
        ButtonPressed.buttonFunctionTable.Add(btnOptions.gameObject.name, OnOptionsButtonClick);
        ButtonPressed.buttonFunctionTable.Add(btnMainMenu.gameObject.name, OnMainMenuButtonClick);
    }


    private void OnResumeButtonClick()
    {//���ذ�ť
        // Time.timeScale = 1f;//ʱ��ָ�
        hUDCanvas.enabled = true;//����hud����
        menusCanvas.enabled = false;//�ر���ͣ�˵�����
        GameManager.GameState = GameState.Playing;//��Ϸ״̬�ָ�Ϊplaying
        TimeController.Instance.UnPause();//ʱ��ָ�
        playInput.EnableGamePlayInput();//�л�����Ϸ������
        playInput.SwitchToFixedUpdateMode();//�������ģʽ �л��ء��̶�������ģʽ
    }
    private void OnOptionsButtonClick()
    {   //ѡ�ťʵ��
        //UIInput.Instance.SelectUI(btnOptions);//ѡ�д�ѡ��
        //playInput.EnablePauseMenuInput();//������ͣ�˵������붯����

        PauseMenu.SetActive(false);//����ѡ�ť
        AudioTuning.SetActive(true);//��������������ť
        UIInput.Instance.SelectUI(btnBack);//ѡ�д�ѡ��
        ButtonPressed.buttonFunctionTable.Add(btnBack.gameObject.name, OnBackButtonClick);//�ֵ������ӷ��ذ�ť����

    }
    private void OnBackButtonClick()
    {//ѡ���еķ��ذ�ť
        AudioTuning.SetActive(false);
        PauseMenu.SetActive(true);
        playInput.EnablePauseMenuInput();//�л�����ͣ�˵�������

        UIInput.Instance.SelectUI(btnResume);//ui�Զ�ѡ�лָ���ť

        //����������а�ť
        ButtonPressed.buttonFunctionTable.Add(btnResume.gameObject.name, OnResumeButtonClick);
        ButtonPressed.buttonFunctionTable.Add(btnOptions.gameObject.name, OnOptionsButtonClick);
        ButtonPressed.buttonFunctionTable.Add(btnMainMenu.gameObject.name, OnMainMenuButtonClick);
    }
    private void OnMainMenuButtonClick()
    {   //���˵���ťʵ��
        menusCanvas.enabled = false;//�ر���ͣ����
        SceneLoader.Instance.LoadScoringScene();// ���ػ��ֳ���
        //SceneLoader.Instance.LoadMainMenuScene();//��������
    }

    private void OnDisable()
    {
        playInput.onPause -= Pause;
        playInput.onUnpause -= Unpause;

        //�л�����ʱ���ע����ֵ�
        ButtonPressed.buttonFunctionTable.Clear();
        //btnResume.onClick.RemoveAllListeners();
        //btnMainMenu.onClick.RemoveAllListeners();
        //btnOptions.onClick.RemoveAllListeners();
    }
    private void Pause()//��Ϸ��ͣ
    {
        //Time.timeScale = 0f;//ʱ����ͣ

        hUDCanvas.enabled = false;//����hud����
        menusCanvas.enabled = true;//������ͣ�˵�����
        GameManager.GameState = GameState.Paused;//��ͣʱ״̬���Ϊpaused
        TimeController.Instance.Pause();//ʱ����ͣ
        playInput.EnablePauseMenuInput();//�л�����ͣ�˵�������
        playInput.SwitchToDynamicUpdateMode();//�������ģʽ �л�������̬������ģʽ����Ȼ���޷�����
        UIInput.Instance.SelectUI(btnResume);//�򿪲˵� ui�Զ�ѡ�лָ���ť
        AudioManager.Instance.PlaySFX(pauseSFX);//��ͣ��Ч
    }
    private void Unpause()//��Ϸ����
    {
        btnResume.Select();
        //����ť״̬�л��ɰ���(��Ϸ�б���Ϊ�ڶ��ΰ���tab���Żָ���ť�Ķ���)
        btnResume.animator.SetTrigger(buttonPressedParameterID);//�����ϣֵ ���ܸ��� ����������޸�
        AudioManager.Instance.PlaySFX(unpauseSFX);//ȡ����ͣ��Ч

    }
}
