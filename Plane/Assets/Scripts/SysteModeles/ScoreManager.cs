using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : PersistentSingLeton<ScoreManager>
{
    #region SCORE DISPLAY
    public int SCORE => score;//���ط����Թ���������
    int score;
    int currentScore;//��ǰֵ
    Vector3 scoreTextScale = new Vector3(1.2f, 1.2f, 1f);//�����ı�����ֵ
    public void ResetScore()
    {//��������
        score = 0;
        currentScore = 0;
        Scoretext.UpdateText(score);
    }
    public void Addscore(int scorePoint)
    {//��������
        currentScore += scorePoint;//��ҷ���+�÷�ֵ
        StartCoroutine(nameof(AddScoreCoroutine));
    }
    IEnumerator AddScoreCoroutine()
    {
        Scoretext.ScaleText(scoreTextScale);//�Ŵ��ı�
        //��̬���ӷ���
        while (score < currentScore)
        {
            score++;
            Scoretext.UpdateText(score);
            yield return null;
        }
        Scoretext.ScaleText(Vector3.one);//��ԭ�ı���С

    }
    #endregion

    #region HIGH SCORE SYSTEM

    [System.Serializable]
    public class Playerscore
    {//��ҵ÷���
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
    {//��ҵ÷�������
        public List<Playerscore> list = new List<Playerscore>();
    }

    readonly string SaveFileName = "player_score.json";//�ļ���

    string Name = "������";//Ĭ�������

    //�¸߷�
    public bool HasNewHightScore => score > LoadPlayerScoreData().list[9].score;//�������ڵ�ʮ���ķ���

    public void SetPlayerName(string newName)
    {//�����������
        Name = newName;
    }

    public void SavePlayerScroeData()
    {//������ҷ�������
        var playerScoreData = LoadPlayerScoreData();//�洢��ȡ������
        playerScoreData.list.Add(new Playerscore(score, Name));
        playerScoreData.list.Sort((x, y) => y.score.CompareTo(x.score));//������x.CompareTo(y)������

        SaveSystem.Save(SaveFileName, playerScoreData);//�÷����ݴ����ļ�
    }

    public PlayerScoreData LoadPlayerScoreData()
    {//��ȡ��ҷ�������
        var playerScoreData = new PlayerScoreData();

        if (SaveSystem.SaveFileExists(SaveFileName))
        {//������д浵�����ȡ�浵�ļ� SaveSystem.Load<PlayerScoreData>(�ļ���)
            playerScoreData = SaveSystem.Load<PlayerScoreData>(SaveFileName);
        }
        else
        {//���û�д浵
            //ʮ�����ݣ�ѭ������ʮ��
            while (playerScoreData.list.Count < 10)
            {
                playerScoreData.list.Add(new Playerscore(0, Name));
            }
            SaveSystem.Save(SaveFileName, playerScoreData);//(�ļ���������)
        }
        return playerScoreData;//���ص÷�����
    }

    #endregion
}
