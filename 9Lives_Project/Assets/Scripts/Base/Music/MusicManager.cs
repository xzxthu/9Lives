using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public class MusicManager : BaseManager<MusicManager>
{
    private List<AudioSource> bgmList = new List<AudioSource>();
    private List<AudioSource> seList = new List<AudioSource>();

    Dictionary<string, AudioClip> bgmDic = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> seDic = new Dictionary<string, AudioClip>();

    private float bgmValue = 1;
    private float seValue = 1;

    // cache
    private string bgmName;
    
    public MusicManager()
    {
        MonoManager.GetInstance().AddUpdateListener(Update);
        for (int i = 0; i < 11; ++i)
        {
            PoolManager.GetInstance().GetObject("BGM/BGMPlayer",
                (res => { PoolManager.GetInstance().PushObject("BGM/BGMPlayer", res); }));
            PoolManager.GetInstance()
                .GetObject("SE/SE", (res => { PoolManager.GetInstance().PushObject("SE/SE", res); }));
        }
    }

    public void Update()
    {
        for (int i = seList.Count - 1; i >= 0; --i)
        {
            if (seList[i] != null && !seList[i].isPlaying && seList[i].clip == null) //seList[i].clip != null
            {
                PoolManager.GetInstance().PushObject("SE/SE", seList[i].gameObject);
                seList.RemoveAt(i);
            }
        }

        for (int i = seList.Count - 1; i >= 0; --i)
        {
            if (seList[i] == null)
                seList.RemoveAt(i);
        }

        for (int i = bgmList.Count - 1; i >= 0; --i)
        {
            if (bgmList[i] != null && !bgmList[i].isPlaying && bgmList[i].clip != null)
            {
                PoolManager.GetInstance().PushObject("BGM/BGMPlayer", bgmList[i].gameObject);
                bgmList.RemoveAt(i);
            }
        }

        for (int i = bgmList.Count - 1; i >= 0; --i)
        {
            if (bgmList[i] == null)
                bgmList.RemoveAt(i);
        }
    }


    public void PlayBGM(string name, bool isLoop = false, bool isCoverAudio = false)
    {
        if (isCoverAudio)
        {
            StopAllSeAndBgm();
        }

        for (int i = 0; i < bgmList.Count; ++i)
        {
            bgmName = bgmList[i].clip.name;
            if (bgmName.Equals(name))
            {
                bgmList[i].Stop();
            }
        }
        

        PoolManager.GetInstance().GetObject("BGM/BGMPlayer", (bgmPlayer) =>
        {
            AudioSource source = bgmPlayer.GetComponent<AudioSource>();
            source.volume = bgmValue;
            source.loop = isLoop;
            if (bgmDic.ContainsKey(name))
            {
                source.clip = bgmDic[name];
                source.Play();
                bgmList.Add(source);
            }
            else
            {
                ResManager.GetInstance().LoadAsync<AudioClip>("Music/BGM/" + name, (audio) =>
                {
                    source.clip = audio;
                    bgmDic.Add(name, audio);
                    source.Play();
                    bgmList.Add(source);
                });
            }
        });
    }


    public void PauseBGM(string name)
    {
        foreach (AudioSource bgm in bgmList)
        {
            if (bgm.clip.name.Equals(name))
            {
                bgm.Pause();
            }
        }
    }

    public void StopBGM(string name)
    {
        for (int i = 0; i < bgmList.Count; ++i)
        {
            if (bgmList[i].clip.name.Equals(name))
            {
                bgmList[i].Stop();
            }
        }
    }


    public void RestartAllBGM(string name)
    {
        foreach (AudioSource bgm in bgmList)
        {
            if (bgm != null)
            {
                bgm.Play();
            }
        }
    }

    public void RestartBGM(string name)
    {
        for (int i = 0; i < bgmList.Count; ++i)
        {
            if (bgmList[i].clip.name.Equals(name))
            {
                bgmList[i].Play();
            }
        }
    }

    public void ReplayBGM(string name)
    {
        foreach (AudioSource bgm in bgmList)
        {
            if (bgm != null)
            {
                bgm.UnPause();
            }
        }
    }



    public void RestartAllSE(string name)
    {
        foreach (AudioSource se in seList)
        {
            if (se != null && se.clip.name.Equals(name))
            {
                se.Play();
            }
        }
    }

    public void ReplaySE(string name)
    {
        foreach (AudioSource se in seList)
        {
            if (se != null && se.clip.name.Equals(name))
            {
                se.UnPause();
            }
        }
    }


    public void SetAllBGMValue(float newVolume)
    {
        bgmValue = newVolume;
        foreach (AudioSource bgm in bgmList)
        {
            bgm.volume = bgmValue;
        }
    }


    public void SetBGMValue(string name, float newVolume)
    {
        for (int i = 0; i < bgmList.Count; ++i)
        {
            if (bgmList[i].clip.name.Equals(name))
            {
                bgmList[i].volume = newVolume;
            }
        }
    }



    public void PlaySE(string name, bool isLoop, Transform targetTransform, bool isCoverAudio = false,
        float spatialBlend = 1,
        UnityAction<AudioSource> callback = null)
    {
        if (isCoverAudio)
        {
            StopAllSeAndBgm();
        }

        PoolManager.GetInstance().GetObject("SE/SE", (res) =>
        {
            res.transform.position = Vector3.zero;
            if (targetTransform)
            {
                res.transform.parent = targetTransform;
                res.transform.localPosition = Vector3.zero;
            }

            AudioSource source = res.GetComponent<AudioSource>();

            source.volume = seValue;
            source.loop = isLoop;
            source.spatialBlend = spatialBlend;
            // source.maxDistance = 500f;
            // source.minDistance = 1f;

            if (seDic.ContainsKey(name))
            {
                source.clip = seDic[name];
                source.Play();
                seList.Add(source);
                if (callback != null)
                {
                    callback(source);
                }
            }
            else
            {
                ResManager.GetInstance().LoadAsync<AudioClip>("Music/SE/" + name, (audio) =>
                {
                    source.clip = audio;
                    seDic.Add(name, audio);
                    source.Play();
                    seList.Add(source);
                    if (callback != null)
                    {
                        callback(source);
                    }
                });
            }
        });
    }

    public void StopSE(string name)
    {
        for (int i = 0; i < seList.Count; ++i)
        {
            if (seList[i].clip.name.Equals(name))
            {
                seList[i].Stop();
            }
        }
    }

    public void SetAllSEValue(float newVolume)
    {
        seValue = newVolume;
        foreach (AudioSource se in seList)
        {
            se.volume = seValue;
        }
    }

    public void SetSEValue(string name, float newVolume)
    {
        for (int i = 0; i < seList.Count; ++i)
        {
            if (seList[i].clip.name.Equals(name))
            {
                seList[i].volume = newVolume;
            }
        }
    }

    public void StopAllSeAndBgm()
    {
        foreach (AudioSource se in seList)
        {
            if (se != null)
            {
                se.Stop();
            }
        }

        foreach (AudioSource bgm in bgmList)
        {
            if (bgm != null)
            {
                bgm.Stop();
            }
        }
    }

    public void PauseAllSeAndBgm()
    {
        foreach (AudioSource se in seList)
        {
            if (se != null)
            {
                se.Pause();
            }
        }

        foreach (AudioSource bgm in bgmList)
        {
            if (bgm != null)
            {
                bgm.Pause();
            }
        }
    }

    public void PauseSE(string name)
    {
        foreach (AudioSource se in seList)
        {
            if (se.clip.name.Equals(name))
            {
                se.Pause();
            }
        }
    }



    public void UnPauseAllSeAndBgm()
    {

        foreach (AudioSource se in seList)
        {
            if (se != null)
            {
                se.UnPause();
            }
        }

        foreach (AudioSource bgm in bgmList)
        {
            // Debug.Log(bgm.clip);
            if (bgm != null)
            {
                bgm.UnPause();
            }
        }
    }

    public int GetCurrentPlayBGMTime(string audioClipName)
    {
        for (int i = 0; i < bgmList.Count; ++i)
        {
            if (bgmList[i].clip.name.Equals(audioClipName))
            {
                return Mathf.RoundToInt(bgmList[i].time);
            }
        }

        return 0;
    }

    public float GetCurrentPlaySETime(string audioClipName)
    {
        for (int i = 0; i < seList.Count; ++i)
        {
            if (seList[i].clip.name.Equals(audioClipName))
            {
                return Mathf.RoundToInt(seList[i].time);
            }
        }

        return 0;
    }

    public float GetBGMLength(string audioClipName)
    {
        for (int i = 0; i < bgmList.Count; ++i)
        {
            if (bgmList[i].clip.name.Equals(audioClipName))
            {
                return bgmList[i].clip.length;
            }
        }

        return 0;
    }

    public float GeSELength(string audioClipName)
    {
        for(int i = 0; i < seList.Count; ++i)
        {
            if (seList[i].clip.name.Equals(audioClipName))
            {
                return seList[i].clip.length;
            }
        }

        return 0;
    }

    public float GetBGMVolume()
    {
        return bgmValue;
    }

    public float GetSEVolume()
    {
        return seValue;
    }

    public void Clear()
    {
        seList.Clear();
        bgmList.Clear();
    }
}