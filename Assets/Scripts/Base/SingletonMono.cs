using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 继承Mono的单例基类
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonMonoBase<T> : MonoBehaviour where T: MonoBehaviour
{
    protected SingletonMonoBase(){}

    private static T instance;
	
    //属性方式
    public static T Instance
    {
        get
        {
            if(!instance)
                instance = FindObjectOfType<T>();			
            return instance;
        }
    }
}
