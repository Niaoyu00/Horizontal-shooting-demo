using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootItem : MonoBehaviour
{
    [SerializeField] float minSpeed = 5f;//�����ƶ��ٶ�
    [SerializeField] float maxSpeed = 15f;//�����ƶ��ٶ�
    [SerializeField] protected AudioData defaultPickUpSFX;//Ĭ��ʰȡ������Ч

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
        lootMessage = GetComponentInChildren<Text>(true);//true:����
    }
    void OnEnable()
    {
        //ս��Ʒ�Զ��������Э��
        StartCoroutine(MoveCoroutine());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PickUp();
    }
    protected virtual void PickUp()
    {//ʰȡս��Ʒ

        StopAllCoroutines();//ֹͣ�������Э��
        animator.Play(pickUpStateID);//����ʰȡ��Ч
        AudioManager.Instance.PlayRandomSFX(pickUpSFX);//����Ĭ��ʰȡ��Ч
    }

    IEnumerator MoveCoroutine()
    {//ս��Ʒ�Զ�������� 

        float speed = Random.Range(minSpeed, maxSpeed);//�����ȡ�ƶ��ٶ�
        Vector3 direction = Vector3.left;//Ĭ���������
        while (true)
        {
            if (player.isActiveAndEnabled)
            {//��ҽű���������״̬ʱ,�����λ�÷���

                direction = (player.transform.position - transform.position).normalized;
            }
            transform.Translate(direction * speed * Time.deltaTime);
            yield return null;
        }
    }
}
