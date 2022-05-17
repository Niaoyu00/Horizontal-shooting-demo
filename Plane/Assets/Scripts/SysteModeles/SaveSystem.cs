using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static void Save(string saveFileName, object data)
    {//�洢���ļ�
        var json = JsonUtility.ToJson(data);
        //Application.persistentDataPath���ٷ�api��ͬ�豸�洢���ض�λ��
        var path = Path.Combine(Application.persistentDataPath, saveFileName);//�ϲ���һ��·��
        try
        {
            File.WriteAllText(path, json);//(·������,���ݲ���)
#if UNITY_EDITOR
            Debug.Log($"�洢�ɹ�����ŵ�ַ��{path}");
#endif
        }
        catch (System.Exception ex)
        {
#if UNITY_EDITOR
            Debug.LogError($"�洢ʧ�ܣ���ŵ�ַ��{path},\n{ex}");
#endif
        }
    }

    public static T Load<T>(string saveFileName)
    {//��ȡ�ļ�
        var path = Path.Combine(Application.persistentDataPath, saveFileName);//�ϲ���һ��·��
        try
        {
            var json = File.ReadAllText(path);//��ȡ�ļ�
            var data = JsonUtility.FromJson<T>(json);//��jsonת�ɷ�������

            return data;
        }
        catch (System.Exception ex)
        {
#if UNITY_EDITOR
            Debug.LogError($"��ȡʧ�ܣ���ŵ�ַ��{path},\n{ex}");
#endif
            return default;
        }
    }

    public static void DeleteSaveFile(string saveFileName)
    {//ɾ���ļ�
        var path = Path.Combine(Application.persistentDataPath, saveFileName);//�ϲ���һ��·��
        try
        {
            File.Delete(path);
        }
        catch (System.Exception ex)
        {
#if UNITY_EDITOR
            Debug.LogError($"ɾ��ʧ�ܣ���ŵ�ַ��{path},\n{ex}");
#endif
        }
    }

    public static bool SaveFileExists(string saveFileName)
    {//���浵�ļ��Ƿ����
        var path = Path.Combine(Application.persistentDataPath, saveFileName);//�ϲ���һ��·��
        return File.Exists(path);//File.Exists����ļ��Ƿ����
    }
}

