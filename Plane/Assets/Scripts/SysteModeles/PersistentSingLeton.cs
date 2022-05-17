using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSingLeton<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }
    protected virtual void Awake()
    {
        if (Instance == null)
        {//���Ϊ�����赱ǰ������
            Instance = this as T;
        }
        else if (Instance != this)
        {//������������ ����������³����������˴˽ű� ��ô���ٵ�ԭ��ԭ���Ķ��� 
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);//�����³���ʱ�����ٴ���Ķ���
    }
}
