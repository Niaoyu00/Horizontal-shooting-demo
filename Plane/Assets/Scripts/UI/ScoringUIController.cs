using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringUIController : MonoBehaviour
{
    [Header("==== BACKGROUND ====")]

    [SerializeField] Image background;
    [SerializeField] Sprite[] backgroundImage;

    [Header("==== SCORING SCREEN ====")]

    [SerializeField] Canvas scoringScreenCanvas;//分数画布
    [SerializeField] Text playerScoreText;//分数
    [SerializeField] Button btnMainMenu;//主菜单按钮
    [SerializeField] Transform hightScoreLeaderboardContainer;//高分排行榜容器

    [Header("==== HIGH SCORE SCREEN ====")]

    [SerializeField] Canvas newHightScoreScreenCanvas;
    [SerializeField] Button btnCancel;
    [SerializeField] Button btnSubmit;
    [SerializeField] InputField playerNameInputField;
    void Start()
    {
        Cursor.visible = true;//显示光标
        Cursor.lockState = CursorLockMode.None;
        ShowRandomBackground();
        if (ScoreManager.Instance.HasNewHightScore)
        {//如果新纪录进榜，显示新纪录画面
            ShowNewHightScoreScreen();
        }
        else
        {
            ShowScoringScreen();//显示积分画面
        }
        //ShowScoringScreen();
        //登记按钮功能 <key:按钮名，value：功能函数>
        ButtonPressed.buttonFunctionTable.Add(btnMainMenu.gameObject.name, OnButtonMainMenuClicked);
        ButtonPressed.buttonFunctionTable.Add(btnSubmit.gameObject.name, OnButtonSubmitClicked);
        ButtonPressed.buttonFunctionTable.Add(btnCancel.gameObject.name, HideNewHighScoreScreen);

        //改变游戏状态
        GameManager.GameState = GameState.Scoring;
    }
    void OnDisable()
    {
        ButtonPressed.buttonFunctionTable.Clear();
    }
    void ShowNewHightScoreScreen()
    {//显示新纪录分数ui画布
        newHightScoreScreenCanvas.enabled = true;//开启新纪录ui画布
        UIInput.Instance.SelectUI(btnCancel);//默认选中取消按钮
    }
    void HideNewHighScoreScreen()
    {//隐藏新纪录分数ui画布
        newHightScoreScreenCanvas.enabled = false;
        //保存玩家分数数据并降序排序前十
        ScoreManager.Instance.SavePlayerScroeData();
        ShowRandomBackground();//随机背景
        ShowScoringScreen();//显示积分画面
    }

    void ShowRandomBackground()
    {//随机显示背景图
        background.sprite = backgroundImage[Random.Range(0, backgroundImage.Length)];
    }
    void ShowScoringScreen()
    {//显示分数ui
        scoringScreenCanvas.enabled = true;//开启画布
        playerScoreText.text = ScoreManager.Instance.SCORE.ToString();//在ScoreManager获取玩家得分
        UIInput.Instance.SelectUI(btnMainMenu);//选中主菜单按钮

        //更新高分排行榜ui
        UpdateHightScoreLeaderboard();
    }

    void UpdateHightScoreLeaderboard()
    {//更新高分排行榜ui
        //得分数据列表
        var playerScoreList = ScoreManager.Instance.LoadPlayerScoreData().list;

        for (int i = 0; i < hightScoreLeaderboardContainer.childCount; i++)
        {
            var child = hightScoreLeaderboardContainer.GetChild(i);//获取孩子对象

            child.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();
            child.Find("Score").GetComponent<Text>().text = playerScoreList[i].score.ToString();
            child.Find("Name").GetComponent<Text>().text = playerScoreList[i].playerName;
        }
    }

    void OnButtonMainMenuClicked()
    {//主菜单按钮点击事件
        scoringScreenCanvas.enabled = false;//关闭ui
        SceneLoader.Instance.LoadMainMenuScene();//加载主菜单场景
    }
    void OnButtonSubmitClicked()
    {//点击Submit
        if (!string.IsNullOrEmpty(playerNameInputField.text))
        {//若文本输入框内容不为空 则更改玩家名字
            ScoreManager.Instance.SetPlayerName(playerNameInputField.text);
        }
        HideNewHighScoreScreen();//关闭窗口（等同取消按钮的功能
    }

}
