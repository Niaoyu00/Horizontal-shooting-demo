using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [Header("===== CANVAS =====")]

    [SerializeField] Canvas mainMenuCanvas;

    [Header("===== BUTTONS =====")]

    [SerializeField] Button btnStart;
    [SerializeField] Button btnOptions;
    [SerializeField] Button btnQuit;

    private void OnEnable()
    {
        ButtonPressed.buttonFunctionTable.Add(btnStart.gameObject.name, OnButtonStartClicked);
        ButtonPressed.buttonFunctionTable.Add(btnOptions.gameObject.name, OnButtonOptionsClicked);
        ButtonPressed.buttonFunctionTable.Add(btnQuit.gameObject.name, OnButtonQuitClicked);
    }
    private void Start()
    {
        Time.timeScale = 1;
        GameManager.GameState = GameState.Playing;//游戏状态设为playing
        UIInput.Instance.SelectUI(btnStart);//默认选中开始游戏按钮
    }
    private void OnDisable()
    {
        ButtonPressed.buttonFunctionTable.Clear();
    }
    void OnButtonStartClicked()
    {
        mainMenuCanvas.enabled = false;//当开始按钮按下，将主菜单ui关闭
        SceneLoader.Instance.LoadGameplayScene();//加载gameplay场景
    }
    void OnButtonOptionsClicked()
    {
        UIInput.Instance.SelectUI(btnOptions);//选中按钮

    }
    void OnButtonQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//编辑器退出
#else
        Application.Quit();//退出应用(仅在build之后生效)
#endif

    }
}
