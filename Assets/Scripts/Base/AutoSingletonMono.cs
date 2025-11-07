using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 继承Mono的自动挂载单例基类
/// </summary>
/// <typeparam name="T"></typeparam>
public class AutoSingletonMonoBase<T> : MonoBehaviour where T: MonoBehaviour
{
    protected AutoSingletonMonoBase(){}
	
    private static T instance;

    public static T Instance
    {
        get
        {
            if(!instance)
            {
                // 场景上创建空物体并挂载脚本
                GameObject obj = new GameObject(typeof(T).Name);
                instance = obj.AddComponent<T>();
                //过场景时不移除，保证整个游戏中都存在
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }
    
}
