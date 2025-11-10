using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicMgr : SingletonBase<MusicMgr>
{

    private List<AudioSource> soundList = new List<AudioSource>(); //正在播放的音效列表
    
    private AudioSource bkMusic = null;
    private GameObject soundObj = null;

    private float bkMusicVolume = 1.0f;
    private float soundVolume = 1.0f;

    public MusicMgr()
    {
        MonoManager.Instance.AddUpdateListener(update);
    }
    private void update()
    {
        for (int i = soundList.Count - 1; i >= 0; i--)
        {
            if (!soundList[i].isPlaying)
            {
                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name">背景音乐的名字</param>
    public void PlayBKMusic(string name)
    {
        if (!bkMusic)
        {
            GameObject obj = new GameObject("BKMusic");
            bkMusic = obj.AddComponent<AudioSource>();
        }
        //异步加载背景音乐并在加载完成后播放
        ResManager.Instance.LoadAsync<AudioClip>("Music/BKMusic/"+name, (clip)=>
        {
            bkMusic.clip = clip;
            bkMusic.loop = true;
            bkMusic.volume = bkMusicVolume;
            bkMusic.Play();
        });
    }

    /// <summary>
    /// 改变背景音乐的音量
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeBKVolume(float volume)
    {
        bkMusicVolume = volume;
        if (!bkMusic) return;
        bkMusic.volume = bkMusicVolume;
    }
    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBKMusic()
    {
        if (!bkMusic) return;
        bkMusic.Pause();
    }
    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBKMusic()
    {
        if (!bkMusic) return;
        bkMusic.Stop();
    }

    /// <summary>
    /// 播放特定音效
    /// </summary>
    /// <param name="name">音效名字</param>
    /// <param name="isLoop">音效是否循环</param>
    /// <param name="callback">播放该音效同时要实现的逻辑（可不加）</param>
    public void PlaySound(string name, bool isLoop, UnityAction<AudioSource> callback = null)
    {
        if(!soundObj)
            soundObj = new GameObject("Sounds");
        AudioSource source = soundObj.AddComponent<AudioSource>();
        
        ResManager.Instance.LoadAsync<AudioClip>("Music/Sounds/"+name, (clip)=>
        {
            source.clip = clip;
            source.loop = isLoop;
            source.volume = soundVolume;
            source.Play();
            soundList.Add(source);
            callback?.Invoke(source);
        });
    }
    
    /// <summary>
    /// 改变所有音效的音量
    /// </summary>
    /// <param name="volume">目标音量</param>
    public void ChangeSoundVolume(float volume)
    {
        soundVolume = volume;
        foreach (var sound in soundList)
        {
            sound.volume = soundVolume;
        }
    }

    /// <summary>
    /// 停止某个音效的播放
    /// </summary>
    /// <param name="source">要停止的音效</param>
    public void StopSound(AudioSource source)
    {
        if (soundList.Contains(source))
        {
            soundList.Remove(source);
            source.Stop();
            GameObject.Destroy(source);
        }
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    




}
