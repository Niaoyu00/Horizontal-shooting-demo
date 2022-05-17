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
    [SerializeField] AudioData pauseSFX;//音效

    [SerializeField] AudioData unpauseSFX;

    [Header("===== CANVAS =====")]

    [SerializeField] Canvas hUDCanvas;

    [SerializeField] Canvas menusCanvas;

    //[SerializeField] Canvas AudioTuning;//音量条

    // [SerializeField] Canvas PauseMenu;//暂停菜单

    [Header("===== BUTTON =====")]

    [SerializeField] Button btnResume;//通过ui中事件绑定过

    [SerializeField] Button btnOptions;

    [SerializeField] Button btnMainMenu;

    [SerializeField] Button btnBack;

    [Header("===== GameObject =====")]

    [SerializeField] GameObject AudioTuning;
    [SerializeField] GameObject PauseMenu;


    int buttonPressedParameterID = Animator.StringToHash("Pressed");//转化为哈希值

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
    {//返回按钮
        // Time.timeScale = 1f;//时间恢复
        hUDCanvas.enabled = true;//开启hud界面
        menusCanvas.enabled = false;//关闭暂停菜单画布
        GameManager.GameState = GameState.Playing;//游戏状态恢复为playing
        TimeController.Instance.UnPause();//时间恢复
        playInput.EnableGamePlayInput();//切换到游戏动作表
        playInput.SwitchToFixedUpdateMode();//输入更新模式 切换回【固定】更新模式
    }
    private void OnOptionsButtonClick()
    {   //选项按钮实现
        //UIInput.Instance.SelectUI(btnOptions);//选中此选项
        //playInput.EnablePauseMenuInput();//开启暂停菜单的输入动作表

        PauseMenu.SetActive(false);//禁用选项按钮
        AudioTuning.SetActive(true);//开启音量调整按钮
        UIInput.Instance.SelectUI(btnBack);//选中此选项
        ButtonPressed.buttonFunctionTable.Add(btnBack.gameObject.name, OnBackButtonClick);//字典中增加返回按钮功能

    }
    private void OnBackButtonClick()
    {//选项中的返回按钮
        AudioTuning.SetActive(false);
        PauseMenu.SetActive(true);
        playInput.EnablePauseMenuInput();//切换到暂停菜单动作表

        UIInput.Instance.SelectUI(btnResume);//ui自动选中恢复按钮

        //重新添加所有按钮
        ButtonPressed.buttonFunctionTable.Add(btnResume.gameObject.name, OnResumeButtonClick);
        ButtonPressed.buttonFunctionTable.Add(btnOptions.gameObject.name, OnOptionsButtonClick);
        ButtonPressed.buttonFunctionTable.Add(btnMainMenu.gameObject.name, OnMainMenuButtonClick);
    }
    private void OnMainMenuButtonClick()
    {   //主菜单按钮实现
        menusCanvas.enabled = false;//关闭暂停画布
        SceneLoader.Instance.LoadScoringScene();// 加载积分场景
        //SceneLoader.Instance.LoadMainMenuScene();//场景加载
    }

    private void OnDisable()
    {
        playInput.onPause -= Pause;
        playInput.onUnpause -= Unpause;

        //切换场景时清除注册的字典
        ButtonPressed.buttonFunctionTable.Clear();
        //btnResume.onClick.RemoveAllListeners();
        //btnMainMenu.onClick.RemoveAllListeners();
        //btnOptions.onClick.RemoveAllListeners();
    }
    private void Pause()//游戏暂停
    {
        //Time.timeScale = 0f;//时间暂停

        hUDCanvas.enabled = false;//隐藏hud界面
        menusCanvas.enabled = true;//开启暂停菜单画布
        GameManager.GameState = GameState.Paused;//暂停时状态类改为paused
        TimeController.Instance.Pause();//时间暂停
        playInput.EnablePauseMenuInput();//切换到暂停菜单动作表
        playInput.SwitchToDynamicUpdateMode();//输入更新模式 切换到【动态】更新模式，不然将无法输入
        UIInput.Instance.SelectUI(btnResume);//打开菜单 ui自动选中恢复按钮
        AudioManager.Instance.PlaySFX(pauseSFX);//暂停音效
    }
    private void Unpause()//游戏继续
    {
        btnResume.Select();
        //将按钮状态切换成按下(游戏中表现为第二次按下tab播放恢复按钮的动画)
        btnResume.animator.SetTrigger(buttonPressedParameterID);//传入哈希值 性能更好 代码更容易修改
        AudioManager.Instance.PlaySFX(unpauseSFX);//取消暂停音效

    }
}
