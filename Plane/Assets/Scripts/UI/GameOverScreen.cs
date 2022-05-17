using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] PlayInput input;

    [SerializeField] Canvas HUDCanvas;

    [SerializeField] AudioData confirmGameOver;//��Ƶ

    int exitStateID = Animator.StringToHash("GameOverScreenExit");//��ȡ��Ϸ��������
    Canvas canvas;

    Animator animator;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        animator = GetComponent<Animator>();

        canvas.enabled = false;
        animator.enabled = false;
    }
    private void OnEnable()
    {
        GameManager.onGameOver += OnGameOver;//������Ϸ��������gameoverί��
        input.onConfirmGameOver += OnConfirmGameOver;
    }
    private void OnDisable()
    {
        GameManager.onGameOver -= OnGameOver;
        input.onConfirmGameOver -= OnConfirmGameOver;

    }
    private void OnConfirmGameOver()
    {
        //1.����ȷ����Ϸ��������Ч
        AudioManager.Instance.PlaySFX(confirmGameOver);
        //2.�ٴν��������������
        input.DisableAllInputs();
        //3.������Ϸ��������
        animator.Play(exitStateID);
        //4.֪ͨ�������������ؼƷֳ���
        SceneLoader.Instance.LoadScoringScene();// ���ػ��ֳ���
    }
    private void OnGameOver()
    {
        HUDCanvas.enabled = false;//�ر�hud ui
        canvas.enabled = true;//����gameoveruiչʾ
        animator.enabled = true;//���������ֲ�
        input.DisableAllInputs();//���������������
    }

    void EnableGameOverScreenInput()
    {//�л�������Ϊ��Ϸ����������
        input.EnableGameOverScreenInput();
    }


}
