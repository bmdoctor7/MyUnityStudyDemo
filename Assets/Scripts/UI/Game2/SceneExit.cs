using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneExit : MonoBehaviour
{
    [Tooltip("需要过渡新场景的名称")]
    public string newSceneName;

    private void Start()
    {
        //gameObject.SetActive(false);//开始后不可见
    }

    //当玩家进入触发器
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TransitionInternal();
        }
    }

    //调用场景切换函数
    public void TransitionInternal()
    {
        SceneLoader.Instance.TransitionToScene(newSceneName);
    }
}
