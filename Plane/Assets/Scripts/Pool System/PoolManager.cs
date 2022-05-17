using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] enemyPools;//���˳�����

    [SerializeField] Pool[] playerProjectilePools;//����ӵ�������

    [SerializeField] Pool[] enemyProjectilePools;//�����ӵ�������

    [SerializeField] Pool[] vFXPools;//�Ӿ���ЧԤ��������

    [SerializeField] Pool[] lootItemPols;//ս��Ʒ����

    static Dictionary<GameObject, Pool> dictionary;//�����ֵ� Ԥ���壬�����

    private void Awake()
    {
        dictionary = new Dictionary<GameObject, Pool>();//��ʼ���ֵ�

        Initialize(enemyPools);//��ʼ�����˳س�
        Initialize(playerProjectilePools);//��ʼ������ӵ���
        Initialize(enemyProjectilePools);//��ʼ�������ӵ���
        Initialize(vFXPools);//��ʼ���Ӿ���ЧԤ�����
        Initialize(lootItemPols);//��ʼ���Ӿ���ЧԤ�����
    }
#if UNITY_EDITOR
    void OnDestroy()
    {//�༭��ֹͣ����ʱ����
        CheckPoolSize(playerProjectilePools);//�ߴ���(һ���������������)
        CheckPoolSize(enemyProjectilePools);//�ߴ���
        CheckPoolSize(vFXPools);//�ߴ���
        CheckPoolSize(enemyPools);//�ߴ���
        CheckPoolSize(lootItemPols);//�ߴ���
    }

#endif

    void CheckPoolSize(Pool[] pools)
    {   //����������гߴ�
        foreach (var item in pools)
        {
            if (item.RuntimeSize > item.Size)
            {
                Debug.LogWarning(string.Format("pool:{0},����ʱ�ߴ�{1},����Ԥ��ֵ����سߴ�{2}",
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

#if UNITY_EDITOR//���༭����ִ��

            //����ֵ��key�Ѿ�����ͬ��Ԥ���� ����ѭ������ֹ�ظ����
            if (dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("�ڶ��������з�����ͬԤ����:" + pool.Prefab.name);
                continue;
            }
#endif

            dictionary.Add(pool.Prefab, pool);//���ֵ������key��value

            Transform poolParent = new GameObject("Pool:" + pool.Prefab.name).transform;//�б���������������pool

            poolParent.parent = transform;//��ǰ����ֵ���������ĸ���

            pool.Initialize(poolParent);

        }
    }
    /// <summary>
    /// <para>Release a specified prepared gameObject in the pool at specified position and rotation.</para>
    /// <para>���ݴ����prefab������rotation��������position����λ�á��ͷš��������Ԥ���õ���Ϸ����</para> 
    /// </summary>
    /// <param name="prefab">
    /// <para>Specified gameObject prefab.</para>
    /// <para>ָ������Ϸ����Ԥ���塣</para>
    /// </param>
    /// <param name="position">
    /// <para>Specified release position.</para>
    /// <para>ָ���ͷ�λ�á�</para>
    /// </param>
    /// <param name="rotation">
    /// <para>Specified rotation.</para>
    /// <para>ָ������תֵ��</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab)
    {//�þ�̬�������㱻���������

#if UNITY_EDITOR
        //���û���ҵ�key����null
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("����ع������Ҳ�����prefab��" + prefab.name);
            return null;
        }
#endif

        return dictionary[prefab].PreparedObject();//ȡ�ö���
    }

    /// <summary>
    /// <para>Release a specified prepared gameObject in the pool at specified position and rotation.</para>
    /// <para>���ݴ����prefab������rotation��������position����λ�á��ͷš��������Ԥ���õ���Ϸ����</para> 
    /// </summary>
    /// <param name="prefab">
    /// <para>Specified gameObject prefab.</para>
    /// <para>ָ������Ϸ����Ԥ���塣</para>
    /// </param>
    /// <param name="position">
    /// <para>Specified release position.</para>
    /// <para>ָ���ͷ�λ�á�</para>
    /// </param>
    /// <param name="rotation">
    /// <para>Specified rotation.</para>
    /// <para>ָ������תֵ��</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position)
    {
        //�þ�̬�������㱻���������

#if UNITY_EDITOR
        //���û���ҵ�key����null
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("����ع������Ҳ�����prefab��" + prefab.name);
            return null;
        }
#endif

        return dictionary[prefab].PreparedObject(position);//ȡ�ö���
    }

    /// <summary>
    /// <para>Release a specified prepared gameObject in the pool at specified position and rotation.</para>
    /// <para>���ݴ����prefab������rotation��������position����λ�á��ͷš��������Ԥ���õ���Ϸ����</para> 
    /// </summary>
    /// <param name="prefab">
    /// <para>Specified gameObject prefab.</para>
    /// <para>ָ������Ϸ����Ԥ���塣</para>
    /// </param>
    /// <param name="position">
    /// <para>Specified release position.</para>
    /// <para>ָ���ͷ�λ�á�</para>
    /// </param>
    /// <param name="rotation">
    /// <para>Specified rotation.</para>
    /// <para>ָ������תֵ��</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        //�þ�̬�������㱻���������

#if UNITY_EDITOR
        //���û���ҵ�key����null
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("����ع������Ҳ�����prefab��" + prefab.name);
            return null;
        }
#endif

        return dictionary[prefab].PreparedObject(position, rotation);//ȡ�ö���
    }
    /// <summary>
    /// <para>Release a specified prepared gameObject in the pool at specified position and rotation.</para>
    /// <para>���ݴ����prefab������rotation��������position����λ�á��ͷš��������Ԥ���õ���Ϸ����</para> 
    /// </summary>
    /// <param name="prefab">
    /// <para>Specified gameObject prefab.</para>
    /// <para>ָ������Ϸ����Ԥ���塣</para>
    /// </param>
    /// <param name="position">
    /// <para>Specified release position.</para>
    /// <para>ָ���ͷ�λ�á�</para>
    /// </param>
    /// <param name="rotation">
    /// <para>Specified rotation.</para>
    /// <para>ָ������תֵ��</para>
    /// </param>
    /// /// <param name="localScale">
    /// <para>Specified scale.</para>
    /// <para>ָ��������ֵ��</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        //�þ�̬�������㱻���������

#if UNITY_EDITOR
        //���û���ҵ�key����null
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("����ع������Ҳ�����prefab��" + prefab.name);
            return null;
        }
#endif

        return dictionary[prefab].PreparedObject(position, rotation, localScale);//ȡ�ö���
    }

}
