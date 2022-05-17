using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] PlayInput input;

    [SerializeField] Canvas HUDCanvas;

    [SerializeField] AudioData confirmGameOver;//音频

    int exitStateID = Animator.StringToHash("GameOverScreenExit");//获取游戏结束动画
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
        GameManager.onGameOver += OnGameOver;//订阅游戏管理器中gameover委托
        input.onConfirmGameOver += OnConfirmGameOver;
    }
    private void OnDisable()
    {
        GameManager.onGameOver -= OnGameOver;
        input.onConfirmGameOver -= OnConfirmGameOver;

    }
    private void OnConfirmGameOver()
    {
        //1.播放确认游戏结束的音效
        AudioManager.Instance.PlaySFX(confirmGameOver);
        //2.再次禁用玩家所有输入
        input.DisableAllInputs();
        //3.播放游戏结束动画
        animator.Play(exitStateID);
        //4.通知场景加载器加载计分场景
        SceneLoader.Instance.LoadScoringScene();// 加载积分场景
    }
    private void OnGameOver()
    {
        HUDCanvas.enabled = false;//关闭hud ui
        canvas.enabled = true;//开启gameoverui展示
        animator.enabled = true;//开启动画轮播
        input.DisableAllInputs();//禁用所有玩家输入
    }

    void EnableGameOverScreenInput()
    {//切换动作表为游戏结束动作表
        input.EnableGameOverScreenInput();
    }


}
