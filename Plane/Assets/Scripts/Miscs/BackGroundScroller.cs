using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroller : MonoBehaviour
{
    Material material;

    [SerializeField] Vector2 scrollVelocity;//SerializeField 暴露出去，不需要使用public 因为这个变量是私有
    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }


    private IEnumerator Start()
    {
        while (GameManager.GameState != GameState.GameOver)//游戏状态不为gameover可运行场景滚动
        {
            material.mainTextureOffset += scrollVelocity * Time.deltaTime;
            yield return null;
        }
    }
}
