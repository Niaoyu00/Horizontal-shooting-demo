
using UnityEngine;

public class SingLeton<T> : MonoBehaviour where T : Component
{//����
    public static T Instance { get; private set; }
    protected virtual void Awake()
    {
        //protected������Ů��������˵������public��
        Instance = this as T;//thisת����T����
    }
}
