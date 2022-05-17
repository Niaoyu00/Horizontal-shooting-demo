using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentSingLeton<GameManager>
{
    public static System.Action onGameOver;//����ί��

    public static GameState GameState { get => Instance.gameState; set => Instance.gameState = value; }

    [SerializeField] GameState gameState = GameState.Playing;
}
public enum GameState
{
    Playing,//��Ϸ������
    Paused,//��ͣ״̬
    GameOver,//��Ϸ����
    Scoring//����״̬
}
