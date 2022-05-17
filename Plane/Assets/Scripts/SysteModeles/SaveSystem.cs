using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static void Save(string saveFileName, object data)
    {//存储到文件
        var json = JsonUtility.ToJson(data);
        //Application.persistentDataPath：官方api不同设备存储在特定位置
        var path = Path.Combine(Application.persistentDataPath, saveFileName);//合并成一个路径
        try
        {
            File.WriteAllText(path, json);//(路径参数,内容参数)
#if UNITY_EDITOR
            Debug.Log($"存储成功，存放地址：{path}");
#endif
        }
        catch (System.Exception ex)
        {
#if UNITY_EDITOR
            Debug.LogError($"存储失败，存放地址：{path},\n{ex}");
#endif
        }
    }

    public static T Load<T>(string saveFileName)
    {//读取文件
        var path = Path.Combine(Application.persistentDataPath, saveFileName);//合并成一个路径
        try
        {
            var json = File.ReadAllText(path);//读取文件
            var data = JsonUtility.FromJson<T>(json);//将json转成泛型数据

            return data;
        }
        catch (System.Exception ex)
        {
#if UNITY_EDITOR
            Debug.LogError($"读取失败，存放地址：{path},\n{ex}");
#endif
            return default;
        }
    }

    public static void DeleteSaveFile(string saveFileName)
    {//删除文件
        var path = Path.Combine(Application.persistentDataPath, saveFileName);//合并成一个路径
        try
        {
            File.Delete(path);
        }
        catch (System.Exception ex)
        {
#if UNITY_EDITOR
            Debug.LogError($"删除失败，存放地址：{path},\n{ex}");
#endif
        }
    }

    public static bool SaveFileExists(string saveFileName)
    {//检测存档文件是否存在
        var path = Path.Combine(Application.persistentDataPath, saveFileName);//合并成一个路径
        return File.Exists(path);//File.Exists检测文件是否存在
    }
}

