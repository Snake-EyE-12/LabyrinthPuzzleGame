using System;
using System.Collections;
using System.Collections.Generic;
using Guymon.DesignPatterns;
using NaughtyAttributes;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

public class AudioManager : Singleton<AudioManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    [SerializeField] private AudioSource audioSourceSFX;
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private List<AudioComponent> audioComponents = new();

    public void Play(string name)
    {
        var audio = GetClipData(name);
        if (audio.audioType == AudioType.SFX)
        {
            audioSourceSFX.volume = Random.Range(audio.volume.x, audio.volume.y);
            audioSourceSFX.pitch = Random.Range(audio.pitch.x, audio.pitch.y);
            audioSourceSFX.PlayOneShot(audio.audioClip);
            return;
        }
        audioSourceMusic.volume = Random.Range(audio.volume.x, audio.volume.y);
        audioSourceMusic.pitch = Random.Range(audio.pitch.x, audio.pitch.y);
        audioSourceMusic.PlayOneShot(audio.audioClip);
    }

    public void Play(string name, float delay)
    {
        StartCoroutine(PlayLater(name, delay));
    }

    private IEnumerator PlayLater(string name, float delay)
    {
        yield return new WaitForSeconds(delay);
        Play(name);
    }

    public AudioComponent GetClipData(string name)
    {
        foreach (var audio in audioComponents)
        {
            if (audio.audioName.Equals(name)) return audio;
        }

        return null;
    }
}

[System.Serializable]
public class AudioComponent
{
    public string audioName;
    public AudioClip audioClip;
    [MinMaxSlider(0, 2)] public Vector2 volume;
    [MinMaxSlider(0, 2)] public Vector2 pitch;
    public AudioType audioType;
}
public enum AudioType
{
    SFX,
    Music
}