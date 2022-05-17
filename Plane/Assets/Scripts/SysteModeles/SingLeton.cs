
using UnityEngine;

public class SingLeton<T> : MonoBehaviour where T : Component
{//泛型
    public static T Instance { get; private set; }
    protected virtual void Awake()
    {
        //protected对于子女、朋友来说，就是public的
        Instance = this as T;//this转换成T类型
    }
}
