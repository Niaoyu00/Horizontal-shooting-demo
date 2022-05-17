using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//��Ϊpoolδ�̳�MonoBehaviour��������[System.Serializable]�ſ����л�
public class Pool
{
    [SerializeField] GameObject prefab;

    public int Size => size;//����ص�Ԥ��ߴ�
    public int RuntimeSize => queue.Count;//����ʱ�����ʵ�ʳߴ磬���ض���Ԫ�ظ���

    [SerializeField] int size = 1;
    public GameObject Prefab => prefab;
    //public GameObject Prefab
    //{
    //    get =>  prefab;
    //}
    //public GameObject Prefab
    //{
    //    get { return prefab; }
    //}

    Queue<GameObject> queue;

    private Transform parent;
    public void Initialize(Transform parent)
    {//��
        queue = new Queue<GameObject>();

        this.parent = parent;

        for (int i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());//��ĩβ��Ӷ���
        }
    }

    GameObject Copy()
    {   //�������ƵĶ���
        var copy = GameObject.Instantiate(prefab, parent);

        copy.SetActive(false);

        return copy;
    }
    GameObject AvailableObject()
    {   //���ö���
        GameObject availableObject = null;
        if (queue.Count > 0 && !queue.Peek().activeSelf)
        //���в�Ϊ�գ����ҵ�һ��Ԫ�ز���������״̬
        {
            availableObject = queue.Dequeue();//������ǰ��ȡ������
        }
        else
        {
            availableObject = Copy();
        }
        queue.Enqueue(availableObject);//ȡ����֮�����̷��س���

        return availableObject;

    }


    public GameObject PreparedObject()
    {   //׼���õĶ���
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);

        return preparedObject;
    }
    public GameObject PreparedObject(Vector3 position)
    {   //����
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);

        preparedObject.transform.position = position;//λ��

        return preparedObject;
    }
    public GameObject PreparedObject(Vector3 position, Quaternion rotation)
    {   //����
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);

        preparedObject.transform.position = position;//λ��

        preparedObject.transform.rotation = rotation;//��ת

        return preparedObject;
    }
    public GameObject PreparedObject(Vector3 position, Quaternion rotation, Vector3 localScale)
    {   //����
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);

        preparedObject.transform.position = position;//λ��

        preparedObject.transform.rotation = rotation;//��ת

        preparedObject.transform.localScale = localScale;//����

        return preparedObject;
    }

    //public void Return(GameObject gameObject)
    //{
    //    queue.Enqueue(gameObject);
    //}

}
