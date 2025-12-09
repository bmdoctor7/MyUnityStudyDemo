using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScreenFader : SingletonMonoBase<ScreenFader>
{
    private ScreenFader(){}
    
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f; //时长

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    
    //淡入场景
    public IEnumerator FadeSceneIn()
    {
        yield return StartCoroutine(Fade(0f, canvasGroup));
        //禁用淡入淡出的CanvasGroup对象
        canvasGroup.gameObject.SetActive(false);
    }

    //淡出场景
    public IEnumerator FadeSceneOut()
    {
        //启用淡入淡出的CanvasGroup对象
        canvasGroup.gameObject.SetActive(true);

        yield return StartCoroutine(Fade(1f, canvasGroup));
    }

    //淡入淡出实现
    private IEnumerator Fade(float finalAlpha, CanvasGroup canvasGroup)
    {
        //使用DOTween来实现淡入淡出效果
        yield return canvasGroup.DOFade(finalAlpha, fadeDuration).WaitForCompletion();
    }
    

}
