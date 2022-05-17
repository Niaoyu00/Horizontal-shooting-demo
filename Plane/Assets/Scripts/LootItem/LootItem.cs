using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootItem : MonoBehaviour
{
    [SerializeField] float minSpeed = 5f;//道具移动速度
    [SerializeField] float maxSpeed = 15f;//道具移动速度
    [SerializeField] protected AudioData defaultPickUpSFX;//默认拾取道具音效

    protected AudioData pickUpSFX;
    int pickUpStateID = Animator.StringToHash("PickUp");
    Animator animator;

    protected Player player;

    protected Text lootMessage;

    void Awake()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        pickUpSFX = defaultPickUpSFX;
        lootMessage = GetComponentInChildren<Text>(true);//true:禁用
    }
    void OnEnable()
    {
        //战利品自动飞向玩家协程
        StartCoroutine(MoveCoroutine());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PickUp();
    }
    protected virtual void PickUp()
    {//拾取战利品

        StopAllCoroutines();//停止飞向玩家协程
        animator.Play(pickUpStateID);//播放拾取特效
        AudioManager.Instance.PlayRandomSFX(pickUpSFX);//播放默认拾取音效
    }

    IEnumerator MoveCoroutine()
    {//战利品自动飞向玩家 

        float speed = Random.Range(minSpeed, maxSpeed);//随机获取移动速度
        Vector3 direction = Vector3.left;//默认向左飞行
        while (true)
        {
            if (player.isActiveAndEnabled)
            {//玩家脚本处于启动状态时,向玩家位置飞行

                direction = (player.transform.position - transform.position).normalized;
            }
            transform.Translate(direction * speed * Time.deltaTime);
            yield return null;
        }
    }
}
