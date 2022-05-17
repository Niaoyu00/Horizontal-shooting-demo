using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentSingLeton<GameManager>
{
    public static System.Action onGameOver;//声明委托

    public static GameState GameState { get => Instance.gameState; set => Instance.gameState = value; }

    [SerializeField] GameState gameState = GameState.Playing;
}
public enum GameState
{
    Playing,//游戏运行中
    Paused,//暂停状态
    GameOver,//游戏结束
    Scoring//积分状态
}
