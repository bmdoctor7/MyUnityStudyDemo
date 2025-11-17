using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : SingletonBase<SaveSystem>
{
    private SaveSystem(){}

    
    /// <summary>
    /// 存档为Json文件
    /// </summary>
    /// <param name="saveFileName">存档路径(若要显示时间：路径为$"{System.DateTime.Now:yyyy.dd.M HH-mm-ss}.saveFileName")</param>
    /// <param name="data">要保存的数据</param>
    public void SaveByJson(string saveFileName, object data)
    {
        var json = JsonUtility.ToJson(data);
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        
        try
        {
            File.WriteAllText(path, json);
            Debug.Log("保存成功，路径为："+path);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    
    /// <summary>
    /// 读取存档文件
    /// </summary>
    /// <param name="saveFileName">存档路径</param>
    /// <typeparam name="T">读取数据类型</typeparam>
    /// <returns></returns>
    public T LoadFromJson<T>(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        if (!File.Exists(path))
        {
            Debug.LogWarning("存档文件不存在，路径为："+path);
            return default(T);
        }
        
        try
        {
            var json = File.ReadAllText(path);
            T data = JsonUtility.FromJson<T>(json);
            Debug.Log("读取存档成功，路径为："+path);
            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            return default(T);
        }
    }

    
    /// <summary>
    /// 删除存档文件
    /// </summary>
    /// <param name="saveFileName">存档路径</param>
    public void DeleteSaveFile(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("存档文件已删除，路径为："+path);
        }
        else
        {
            Debug.LogWarning("存档文件不存在，无法删除，路径为："+path);
        }
    }
    
    
    
}
