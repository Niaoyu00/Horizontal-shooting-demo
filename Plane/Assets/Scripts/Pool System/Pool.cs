using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//因为pool未继承MonoBehaviour，所以用[System.Serializable]才可序列化
public class Pool
{
    [SerializeField] GameObject prefab;

    public int Size => size;//对象池的预设尺寸
    public int RuntimeSize => queue.Count;//运行时对象池实际尺寸，返回队列元素个数

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
    {//池
        queue = new Queue<GameObject>();

        this.parent = parent;

        for (int i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());//最末尾添加对象
        }
    }

    GameObject Copy()
    {   //用来复制的对象
        var copy = GameObject.Instantiate(prefab, parent);

        copy.SetActive(false);

        return copy;
    }
    GameObject AvailableObject()
    {   //可用对象
        GameObject availableObject = null;
        if (queue.Count > 0 && !queue.Peek().activeSelf)
        //队列不为空，并且第一个元素不是已启用状态
        {
            availableObject = queue.Dequeue();//队列最前端取出对象
        }
        else
        {
            availableObject = Copy();
        }
        queue.Enqueue(availableObject);//取出来之后立刻返回池中

        return availableObject;

    }


    public GameObject PreparedObject()
    {   //准备好的对象
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);

        return preparedObject;
    }
    public GameObject PreparedObject(Vector3 position)
    {   //重载
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);

        preparedObject.transform.position = position;//位移

        return preparedObject;
    }
    public GameObject PreparedObject(Vector3 position, Quaternion rotation)
    {   //重载
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);

        preparedObject.transform.position = position;//位移

        preparedObject.transform.rotation = rotation;//旋转

        return preparedObject;
    }
    public GameObject PreparedObject(Vector3 position, Quaternion rotation, Vector3 localScale)
    {   //重载
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);

        preparedObject.transform.position = position;//位移

        preparedObject.transform.rotation = rotation;//旋转

        preparedObject.transform.localScale = localScale;//缩放

        return preparedObject;
    }

    //public void Return(GameObject gameObject)
    //{
    //    queue.Enqueue(gameObject);
    //}

}
