using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider sfxVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private GameObject menu;

    private void Awake()
    {
        masterVolume.onValueChanged.AddListener(SetMasterVolume);
        musicVolume.onValueChanged.AddListener(SetMusicVolume);
        sfxVolume.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat("Master_Volume", volume);
        masterMixer.SetFloat("Master_Volume", Mathf.Log10(volume) * 20);
    }
    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("Music_Volume", volume);
        masterMixer.SetFloat("Music_Volume", Mathf.Log10(volume) * 20);
    }
    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFX_Volume", volume);
        masterMixer.SetFloat("SFX_Volume", Mathf.Log10(volume) * 20);
    }

    public void Open()
    {
        masterVolume.value = PlayerPrefs.GetFloat("Master_Volume");
        musicVolume.value = PlayerPrefs.GetFloat("Music_Volume");
        sfxVolume.value = PlayerPrefs.GetFloat("SFX_Volume");
        menu.SetActive(true);
    }

    public void Close()
    {
        menu.SetActive(false);
    }


}
