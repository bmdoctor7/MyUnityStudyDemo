using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleController : SingletonMonoBase<TimeScaleController>
{
    private TimeScaleController(){}
    
    private int slowRefCount; // 慢动作请求引用计数
    
    [Range(0f,1f)] public float slowScale = 0.2f;

    //外部调用：全局时间流速变慢
    public void RequestSlow()
    {
        slowRefCount++;
        Apply();
    }
    
    //外部调用：全局时间流速恢复正常
    public void ReleaseSlow()
    {
        slowRefCount = Mathf.Max(0, slowRefCount - 1);
        Apply();
    }

    // 时间流速改变
    private void Apply()
    {
        Time.timeScale = slowRefCount > 0 ? slowScale : 1f;
    }

    // 强制重置时间流速（不考虑引用计数）
    public void ForceReset()
    {
        slowRefCount = 0;
        Apply();
    }
    
    
}
