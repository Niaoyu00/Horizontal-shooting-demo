using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : PersistentSingLeton<ScoreManager>
{
    #region SCORE DISPLAY
    public int SCORE => score;//返回分数以供别的类调用
    int score;
    int currentScore;//当前值
    Vector3 scoreTextScale = new Vector3(1.2f, 1.2f, 1f);//分数文本缩放值
    public void ResetScore()
    {//分数归零
        score = 0;
        currentScore = 0;
        Scoretext.UpdateText(score);
    }
    public void Addscore(int scorePoint)
    {//分数增加
        currentScore += scorePoint;//玩家分数+得分值
        StartCoroutine(nameof(AddScoreCoroutine));
    }
    IEnumerator AddScoreCoroutine()
    {
        Scoretext.ScaleText(scoreTextScale);//放大文本
        //动态增加分数
        while (score < currentScore)
        {
            score++;
            Scoretext.UpdateText(score);
            yield return null;
        }
        Scoretext.ScaleText(Vector3.one);//还原文本大小

    }
    #endregion

    #region HIGH SCORE SYSTEM

    [System.Serializable]
    public class Playerscore
    {//玩家得分类
        public int score;
        public string playerName;
        public Playerscore(int score, string playerName)
        {
            this.score = score;
            this.playerName = playerName;
        }
    }

    [System.Serializable]
    public class PlayerScoreData
    {//玩家得分数据类
        public List<Playerscore> list = new List<Playerscore>();
    }

    readonly string SaveFileName = "player_score.json";//文件名

    string Name = "无名氏";//默认玩家名

    //新高分
    public bool HasNewHightScore => score > LoadPlayerScoreData().list[9].score;//分数大于第十名的分数

    public void SetPlayerName(string newName)
    {//设置玩家名字
        Name = newName;
    }

    public void SavePlayerScroeData()
    {//保存玩家分数数据
        var playerScoreData = LoadPlayerScoreData();//存储读取的数据
        playerScoreData.list.Add(new Playerscore(score, Name));
        playerScoreData.list.Sort((x, y) => y.score.CompareTo(x.score));//排序函数x.CompareTo(y)是升序

        SaveSystem.Save(SaveFileName, playerScoreData);//得分数据存入文件
    }

    public PlayerScoreData LoadPlayerScoreData()
    {//读取玩家分数数据
        var playerScoreData = new PlayerScoreData();

        if (SaveSystem.SaveFileExists(SaveFileName))
        {//如果已有存档，则读取存档文件 SaveSystem.Load<PlayerScoreData>(文件名)
            playerScoreData = SaveSystem.Load<PlayerScoreData>(SaveFileName);
        }
        else
        {//如果没有存档
            //十个数据，循环创建十次
            while (playerScoreData.list.Count < 10)
            {
                playerScoreData.list.Add(new Playerscore(0, Name));
            }
            SaveSystem.Save(SaveFileName, playerScoreData);//(文件名，数据)
        }
        return playerScoreData;//返回得分数据
    }

    #endregion
}
