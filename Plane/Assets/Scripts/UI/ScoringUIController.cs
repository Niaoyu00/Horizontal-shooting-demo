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

    [SerializeField] Canvas scoringScreenCanvas;//��������
    [SerializeField] Text playerScoreText;//����
    [SerializeField] Button btnMainMenu;//���˵���ť
    [SerializeField] Transform hightScoreLeaderboardContainer;//�߷����а�����

    [Header("==== HIGH SCORE SCREEN ====")]

    [SerializeField] Canvas newHightScoreScreenCanvas;
    [SerializeField] Button btnCancel;
    [SerializeField] Button btnSubmit;
    [SerializeField] InputField playerNameInputField;
    void Start()
    {
        Cursor.visible = true;//��ʾ���
        Cursor.lockState = CursorLockMode.None;
        ShowRandomBackground();
        if (ScoreManager.Instance.HasNewHightScore)
        {//����¼�¼������ʾ�¼�¼����
            ShowNewHightScoreScreen();
        }
        else
        {
            ShowScoringScreen();//��ʾ���ֻ���
        }
        //ShowScoringScreen();
        //�Ǽǰ�ť���� <key:��ť����value�����ܺ���>
        ButtonPressed.buttonFunctionTable.Add(btnMainMenu.gameObject.name, OnButtonMainMenuClicked);
        ButtonPressed.buttonFunctionTable.Add(btnSubmit.gameObject.name, OnButtonSubmitClicked);
        ButtonPressed.buttonFunctionTable.Add(btnCancel.gameObject.name, HideNewHighScoreScreen);

        //�ı���Ϸ״̬
        GameManager.GameState = GameState.Scoring;
    }
    void OnDisable()
    {
        ButtonPressed.buttonFunctionTable.Clear();
    }
    void ShowNewHightScoreScreen()
    {//��ʾ�¼�¼����ui����
        newHightScoreScreenCanvas.enabled = true;//�����¼�¼ui����
        UIInput.Instance.SelectUI(btnCancel);//Ĭ��ѡ��ȡ����ť
    }
    void HideNewHighScoreScreen()
    {//�����¼�¼����ui����
        newHightScoreScreenCanvas.enabled = false;
        //������ҷ������ݲ���������ǰʮ
        ScoreManager.Instance.SavePlayerScroeData();
        ShowRandomBackground();//�������
        ShowScoringScreen();//��ʾ���ֻ���
    }

    void ShowRandomBackground()
    {//�����ʾ����ͼ
        background.sprite = backgroundImage[Random.Range(0, backgroundImage.Length)];
    }
    void ShowScoringScreen()
    {//��ʾ����ui
        scoringScreenCanvas.enabled = true;//��������
        playerScoreText.text = ScoreManager.Instance.SCORE.ToString();//��ScoreManager��ȡ��ҵ÷�
        UIInput.Instance.SelectUI(btnMainMenu);//ѡ�����˵���ť

        //���¸߷����а�ui
        UpdateHightScoreLeaderboard();
    }

    void UpdateHightScoreLeaderboard()
    {//���¸߷����а�ui
        //�÷������б�
        var playerScoreList = ScoreManager.Instance.LoadPlayerScoreData().list;

        for (int i = 0; i < hightScoreLeaderboardContainer.childCount; i++)
        {
            var child = hightScoreLeaderboardContainer.GetChild(i);//��ȡ���Ӷ���

            child.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();
            child.Find("Score").GetComponent<Text>().text = playerScoreList[i].score.ToString();
            child.Find("Name").GetComponent<Text>().text = playerScoreList[i].playerName;
        }
    }

    void OnButtonMainMenuClicked()
    {//���˵���ť����¼�
        scoringScreenCanvas.enabled = false;//�ر�ui
        SceneLoader.Instance.LoadMainMenuScene();//�������˵�����
    }
    void OnButtonSubmitClicked()
    {//���Submit
        if (!string.IsNullOrEmpty(playerNameInputField.text))
        {//���ı���������ݲ�Ϊ�� ������������
            ScoreManager.Instance.SetPlayerName(playerNameInputField.text);
        }
        HideNewHighScoreScreen();//�رմ��ڣ���ͬȡ����ť�Ĺ���
    }

}
