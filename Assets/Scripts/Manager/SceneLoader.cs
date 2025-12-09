using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : SingletonMonoBase<SceneLoader>
{
    private SceneLoader(){}

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    //切换场景函数
    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(TransitionCoroutine(sceneName));
    }
    
    
    //切换场景协程
    public IEnumerator TransitionCoroutine(string newSceneName)
    {
        //保存所有持久化数据
        GameManager1.Instance.SaveBackpackData();
        GameManager1.Instance.SaveToolbarData();

        //淡出当前场景
        yield return StartCoroutine(ScreenFader.Instance.FadeSceneOut());

        //异步加载新场景
        yield return SceneManager.LoadSceneAsync(newSceneName);

        //加载所有持久化数据
        GameManager1.Instance.LoadBackpackData();
        GameManager1.Instance.LoadToolbarData();

        //获取目标场景过渡的位置
        SceneEntrance entrance = FindObjectOfType<SceneEntrance>();

        //设置进入游戏对象的位置
        SetEnteringPosition(entrance);

        //淡入新场景
        yield return StartCoroutine(ScreenFader.Instance.FadeSceneIn());
    }


    private void SetEnteringPosition(SceneEntrance entrance)
    {
        if (!entrance)
            return;

        //把目标场景过渡的位置赋给玩家的位置
        Transform entanceTransform = entrance.transform;
        PlayController.Instance.transform.position = entanceTransform.position;
    }
    
    
    
    
}
