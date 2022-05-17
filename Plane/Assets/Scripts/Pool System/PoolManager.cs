using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] enemyPools;//敌人池数组

    [SerializeField] Pool[] playerProjectilePools;//玩家子弹池数组

    [SerializeField] Pool[] enemyProjectilePools;//敌人子弹池数组

    [SerializeField] Pool[] vFXPools;//视觉特效预制体数组

    [SerializeField] Pool[] lootItemPols;//战利品数组

    static Dictionary<GameObject, Pool> dictionary;//定义字典 预制体，对象池

    private void Awake()
    {
        dictionary = new Dictionary<GameObject, Pool>();//初始化字典

        Initialize(enemyPools);//初始化敌人池池
        Initialize(playerProjectilePools);//初始化玩家子弹池
        Initialize(enemyProjectilePools);//初始化敌人子弹池
        Initialize(vFXPools);//初始化视觉特效预制体池
        Initialize(lootItemPols);//初始化视觉特效预制体池
    }
#if UNITY_EDITOR
    void OnDestroy()
    {//编辑器停止运行时调用
        CheckPoolSize(playerProjectilePools);//尺寸检测(一次性生成最多数量)
        CheckPoolSize(enemyProjectilePools);//尺寸检测
        CheckPoolSize(vFXPools);//尺寸检测
        CheckPoolSize(enemyPools);//尺寸检测
        CheckPoolSize(lootItemPols);//尺寸检测
    }

#endif

    void CheckPoolSize(Pool[] pools)
    {   //检测对象池运行尺寸
        foreach (var item in pools)
        {
            if (item.RuntimeSize > item.Size)
            {
                Debug.LogWarning(string.Format("pool:{0},运行时尺寸{1},大于预设值对象池尺寸{2}",
                    item.Prefab.name,
                    item.RuntimeSize,
                    item.Size));
            }

        }
    }

    void Initialize(Pool[] pools)
    {
        foreach (var pool in pools)
        {

#if UNITY_EDITOR//仅编辑器中执行

            //如果字典的key已经有相同的预制体 跳过循环。防止重复添加
            if (dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("在多个对象池中发现相同预制体:" + pool.Prefab.name);
                continue;
            }
#endif

            dictionary.Add(pool.Prefab, pool);//向字典中添加key和value

            Transform poolParent = new GameObject("Pool:" + pool.Prefab.name).transform;//列表创建空容器用来存pool

            poolParent.parent = transform;//当前对象赋值到空容器的父类

            pool.Initialize(poolParent);

        }
    }
    /// <summary>
    /// <para>Release a specified prepared gameObject in the pool at specified position and rotation.</para>
    /// <para>根据传入的prefab参数和rotation参数，在position参数位置【释放】对象池中预备好的游戏对象。</para> 
    /// </summary>
    /// <param name="prefab">
    /// <para>Specified gameObject prefab.</para>
    /// <para>指定的游戏对象预制体。</para>
    /// </param>
    /// <param name="position">
    /// <para>Specified release position.</para>
    /// <para>指定释放位置。</para>
    /// </param>
    /// <param name="rotation">
    /// <para>Specified rotation.</para>
    /// <para>指定的旋转值。</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab)
    {//用静态函数方便被其他类调用

#if UNITY_EDITOR
        //如果没有找到key返回null
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("对象池管理器找不到此prefab：" + prefab.name);
            return null;
        }
#endif

        return dictionary[prefab].PreparedObject();//取得对象
    }

    /// <summary>
    /// <para>Release a specified prepared gameObject in the pool at specified position and rotation.</para>
    /// <para>根据传入的prefab参数和rotation参数，在position参数位置【释放】对象池中预备好的游戏对象。</para> 
    /// </summary>
    /// <param name="prefab">
    /// <para>Specified gameObject prefab.</para>
    /// <para>指定的游戏对象预制体。</para>
    /// </param>
    /// <param name="position">
    /// <para>Specified release position.</para>
    /// <para>指定释放位置。</para>
    /// </param>
    /// <param name="rotation">
    /// <para>Specified rotation.</para>
    /// <para>指定的旋转值。</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position)
    {
        //用静态函数方便被其他类调用

#if UNITY_EDITOR
        //如果没有找到key返回null
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("对象池管理器找不到此prefab：" + prefab.name);
            return null;
        }
#endif

        return dictionary[prefab].PreparedObject(position);//取得对象
    }

    /// <summary>
    /// <para>Release a specified prepared gameObject in the pool at specified position and rotation.</para>
    /// <para>根据传入的prefab参数和rotation参数，在position参数位置【释放】对象池中预备好的游戏对象。</para> 
    /// </summary>
    /// <param name="prefab">
    /// <para>Specified gameObject prefab.</para>
    /// <para>指定的游戏对象预制体。</para>
    /// </param>
    /// <param name="position">
    /// <para>Specified release position.</para>
    /// <para>指定释放位置。</para>
    /// </param>
    /// <param name="rotation">
    /// <para>Specified rotation.</para>
    /// <para>指定的旋转值。</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        //用静态函数方便被其他类调用

#if UNITY_EDITOR
        //如果没有找到key返回null
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("对象池管理器找不到此prefab：" + prefab.name);
            return null;
        }
#endif

        return dictionary[prefab].PreparedObject(position, rotation);//取得对象
    }
    /// <summary>
    /// <para>Release a specified prepared gameObject in the pool at specified position and rotation.</para>
    /// <para>根据传入的prefab参数和rotation参数，在position参数位置【释放】对象池中预备好的游戏对象。</para> 
    /// </summary>
    /// <param name="prefab">
    /// <para>Specified gameObject prefab.</para>
    /// <para>指定的游戏对象预制体。</para>
    /// </param>
    /// <param name="position">
    /// <para>Specified release position.</para>
    /// <para>指定释放位置。</para>
    /// </param>
    /// <param name="rotation">
    /// <para>Specified rotation.</para>
    /// <para>指定的旋转值。</para>
    /// </param>
    /// /// <param name="localScale">
    /// <para>Specified scale.</para>
    /// <para>指定的缩放值。</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        //用静态函数方便被其他类调用

#if UNITY_EDITOR
        //如果没有找到key返回null
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("对象池管理器找不到此prefab：" + prefab.name);
            return null;
        }
#endif

        return dictionary[prefab].PreparedObject(position, rotation, localScale);//取得对象
    }

}
