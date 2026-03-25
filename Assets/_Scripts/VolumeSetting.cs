using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    private float musicVolume;
    private float sfxVolume;

    private void Start()
    {
        if (PlayerPrefs.HasKey(CONSTANT.SFXVolume) && PlayerPrefs.HasKey(CONSTANT.MusicVolume))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    public void SetMusicVolume()
    {
        musicVolume = musicSlider.value;
        audioMixer.SetFloat(CONSTANT.Music, Mathf.Log10(musicVolume) * 20);
    }

    public void SetSFXVolume()
    {
        sfxVolume = sfxSlider.value;
        audioMixer.SetFloat(CONSTANT.SFX, Mathf.Log10(sfxVolume) * 20);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat(CONSTANT.MusicVolume);
        sfxSlider.value = PlayerPrefs.GetFloat(CONSTANT.SFXVolume);

        SetMusicVolume();
        SetSFXVolume();
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat(CONSTANT.MusicVolume, musicVolume);
        PlayerPrefs.SetFloat(CONSTANT.SFXVolume, sfxVolume);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ApplySavedVolume();
        }
    }
}