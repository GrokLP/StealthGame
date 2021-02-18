using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    //make separate array for songs
    public Sounds[] sounds;
    public Music[] music;

    public string[] musicList; //list of song names, where index is the associated level

    string currentMusic;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
        GameManager.OnLoadComplete += CheckCurrentMusic;

        foreach (Music m in music)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;

            m.source.volume = m.volume;
            m.source.loop = m.loop;
        }

        foreach(Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        PlayMusic(musicList[0]); //could maybe make this cleaner? right now I think audiomanager runs before gamemanager levelindex is set, so this works instead
    }

    public void PlaySound(string name)//play effects method
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);

        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }

        s.source.Play();
    }

    public void StopSound(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }

        s.source.Stop();
    }

    public void CheckCurrentMusic()
    {
        if(currentMusic == null)
        {
            PlayMusic(musicList[GameManager.Instance.CurrentLevelIndex]);
        }
        else if(currentMusic == musicList[GameManager.Instance.CurrentLevelIndex])
        {
            return;
        }
        else if(currentMusic != musicList[GameManager.Instance.CurrentLevelIndex])
        {
            PlayMusic(musicList[GameManager.Instance.CurrentLevelIndex]);
        }
    }

    public void PlayMusic(string name)
    {
        if(currentMusic != null)
            StopMusic(currentMusic);
        
        Music m = Array.Find(music, musicTrack => musicTrack.name == name);

        if(m == null)
        {
            Debug.LogWarning("Music: " + name + "not found!");
            return;
        }

        currentMusic = m.name;

        m.source.Play();
    }

    public void StopMusic(string name)
    {
        Music m = Array.Find(music, musicTrack => musicTrack.name == name);

        if (m == null)
        {
            Debug.LogWarning("Music: " + name + "not found!");
            return;
        }

        m.source.Stop();
    }

    //could have track associated with each level, have play music check what is playing on load, and only change track if* it's not the right one?
}
