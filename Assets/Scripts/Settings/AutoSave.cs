using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AutoSaveOnRun
{
    private const string MenuNameOn = "EasyTool/Auto Save/On";
    private const string MenuNameOff = "EasyTool/Auto Save/Off";
    
    [MenuItem(MenuNameOn)]
    private static void AutoSaveOn()
    {
        // 在编辑器模式改变时，触发 AutoSave 方法
        EditorApplication.playModeStateChanged += AutoSave;
        Debug.Log("开启自动保存");
    }
        
    [MenuItem(MenuNameOff)]
    private static void AutoSaveOff()
    {
        // 删除触发事件
        EditorApplication.playModeStateChanged -= AutoSave;
        Debug.Log("关闭自动保存");
    }
    
    
    
    // 用来保存场景的关键函数
    private static void AutoSave(PlayModeStateChange state)
    {
        // 判断编辑器是否正要进入运行模式
        if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
        {
            // 保存当前打开的场景
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();

            // 输出信息方便查看
            Debug.Log("保存成功");
        }
    }

}
