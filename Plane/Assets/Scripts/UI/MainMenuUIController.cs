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
        GameManager.GameState = GameState.Playing;//��Ϸ״̬��Ϊplaying
        UIInput.Instance.SelectUI(btnStart);//Ĭ��ѡ�п�ʼ��Ϸ��ť
    }
    private void OnDisable()
    {
        ButtonPressed.buttonFunctionTable.Clear();
    }
    void OnButtonStartClicked()
    {
        mainMenuCanvas.enabled = false;//����ʼ��ť���£������˵�ui�ر�
        SceneLoader.Instance.LoadGameplayScene();//����gameplay����
    }
    void OnButtonOptionsClicked()
    {
        UIInput.Instance.SelectUI(btnOptions);//ѡ�а�ť

    }
    void OnButtonQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�༭���˳�
#else
        Application.Quit();//�˳�Ӧ��(����build֮����Ч)
#endif

    }
}
