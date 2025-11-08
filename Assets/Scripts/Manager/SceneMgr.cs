using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneMgr : SingletonBase<SceneMgr>
{
    private SceneMgr(){}

    
    /// <summary>
    /// 同步加载场景
    /// </summary>
    /// <param name="name">场景的名字</param>
    /// <param name="func">加载完成后执行的逻辑</param>
    public void LoadScene(string name, UnityAction func)
    {
        SceneManager.LoadScene(name);
        func?.Invoke();
    }
    
    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="name">场景的名字</param>
    /// <param name="func">加载完成后执行的逻辑</param>
    public void LoadSceneAsync(string name, UnityAction func)
    {
        MonoManager.Instance.StartCoroutine(LoadSceneAsyncCoroutine(name, func));
    }
    private IEnumerator LoadSceneAsyncCoroutine(string name, UnityAction func)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        while (!ao.isDone)
        {
            //广播加载进度
            EventManager.Broadcast(EventType.Loading, ao.progress);
            yield return ao.progress;
        }
        func?.Invoke();
    }
    
    
}
