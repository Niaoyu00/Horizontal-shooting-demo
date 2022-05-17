using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSingLeton<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }
    protected virtual void Awake()
    {
        if (Instance == null)
        {//如果为空则赋予当前泛型类
            Instance = this as T;
        }
        else if (Instance != this)
        {//如果不是这个类 例如加载了新场景并挂载了此脚本 那么销毁掉原来原来的对象 
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);//加载新场景时别销毁传入的对象
    }
}
