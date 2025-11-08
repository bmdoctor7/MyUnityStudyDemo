using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMgr : SingletonBase<MusicMgr>
{

    private List<AudioSource> soundList = new List<AudioSource>();
    
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
        
        ResManager.Instance.LoadAsync<AudioClip>("Music/BKMusic/"+name, (clip)=>
        {
            bkMusic.clip = clip;
            bkMusic.loop = true;
            bkMusic.volume = bkMusicVolume;
            bkMusic.Play();
        });
    }
    


}
