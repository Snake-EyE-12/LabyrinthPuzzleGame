using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private void Start()
    {
        masterVolume.onValueChanged.AddListener(SetMasterVolume);
        musicVolume.onValueChanged.AddListener(SetMusicVolume);
        sfxVolume.onValueChanged.AddListener(SetSFXVolume);
        SetSliderValues();
        SetVolumes();
        resolutionDropdown.options = new List<TMP_Dropdown.OptionData>();
        foreach (var resolution in Screen.resolutions)
        {
            if(resolution.height * 1.0f / resolution.width != 0.75f)
                continue;
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.width + " x " + resolution.height));
        }
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    private void SetVolumes()
    {
        SetMasterVolume(PlayerPrefs.GetFloat("Master_Volume"));
        SetMusicVolume(PlayerPrefs.GetFloat("Music_Volume"));
        SetSFXVolume(PlayerPrefs.GetFloat("SFX_Volume"));
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
        SetSliderValues();
        menu.SetActive(true);
    }

    private void SetSliderValues()
    {
        masterVolume.value = PlayerPrefs.GetFloat("Master_Volume");
        musicVolume.value = PlayerPrefs.GetFloat("Music_Volume");
        sfxVolume.value = PlayerPrefs.GetFloat("SFX_Volume");
    }

    public void Close()
    {
        menu.SetActive(false);
    }


}
