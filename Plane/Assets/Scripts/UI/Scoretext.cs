using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoretext : MonoBehaviour
{
    static Text scoreText;
    private void Awake() => scoreText = GetComponent<Text>();
    private void Start() => ScoreManager.Instance.ResetScore();//开局调用分数归零函数
    //更新文本
    public static void UpdateText(int score) => scoreText.text = score.ToString();
    //分数文本缩放
    public static void ScaleText(Vector3 targetScale) => scoreText.rectTransform.localScale = targetScale;
}
