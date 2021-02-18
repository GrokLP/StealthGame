using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Music
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
