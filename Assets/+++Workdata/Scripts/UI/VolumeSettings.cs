using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    /// <summary>
    /// the audio mixer you use
    /// </summary>
    [SerializeField] AudioMixer mixer;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sFXSlider;

    /// <summary>
    /// string to get the master sliderNumber
    /// </summary>
    public const string MASTER_KEY = "masterVolume";

    /// <summary>
    /// string to get the music sliderNumber
    /// </summary>
    public const string MUSIC_KEY = "musicVolume";

    /// <summary>
    /// string to get the sfx sliderNumber
    /// </summary>
    public const string SFX_KEY = "sfxVolume";

    private void Awake()
    {
        if (PlayerPrefs.HasKey(MUSIC_KEY))
        {
            LoadVolume();
        }
        else
        {
            SetMasterVolume();
            SetMusicVolume();
            SetSfxVolume();
        }
    }

    /// <summary>
    /// Load the saved volume settings
    /// </summary>
    void LoadVolume() //Volume saved in VolumeSettings.cs
    {
        masterSlider.value = PlayerPrefs.GetFloat(MASTER_KEY);
        musicSlider.value = PlayerPrefs.GetFloat(MUSIC_KEY);
        sFXSlider.value = PlayerPrefs.GetFloat(SFX_KEY);
        
        SetMasterVolume();
        SetMusicVolume();
        SetSfxVolume();
    }

    /// <summary>
    /// Set the Volume of the master
    /// </summary>
    /// <param name="value">number of the slider</param>
    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        mixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(MASTER_KEY, volume);
    }

    /// <summary>
    /// set the volume of the music 
    /// </summary>
    /// <param name="value">number of the slider</param>
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(MUSIC_KEY, volume);
    }

    /// <summary>
    /// set the volume of the sfx
    /// </summary>
    /// <param name="value">number of the slider</param>
    public void SetSfxVolume()
    {
        float volume = sFXSlider.value;
        mixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(SFX_KEY, volume);
    }
}
