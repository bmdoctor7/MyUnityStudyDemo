using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 不继承Mono的单例基类
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonBase<T> where T: SingletonBase<T>
{
    
    protected SingletonBase(){}
    
    private static T instance;
	
    //属性方式
    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = Activator.CreateInstance(typeof(T), true) as T;
            return instance;
        }
    }
    // //方法的方式
    // public static T GetInstance()
    // {
    //     if(instance == null)
    //         instance = new T();
    //     return instance;
    // }
}
