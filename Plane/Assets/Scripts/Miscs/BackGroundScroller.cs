using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroller : MonoBehaviour
{
    Material material;

    [SerializeField] Vector2 scrollVelocity;//SerializeField ��¶��ȥ������Ҫʹ��public ��Ϊ���������˽��
    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }


    private IEnumerator Start()
    {
        while (GameManager.GameState != GameState.GameOver)//��Ϸ״̬��Ϊgameover�����г�������
        {
            material.mainTextureOffset += scrollVelocity * Time.deltaTime;
            yield return null;
        }
    }
}
