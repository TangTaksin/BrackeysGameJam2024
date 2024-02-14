using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("==============Settings==============")]
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    void Awake()
    {
        if (PlayerPrefs.HasKey("musicVolume") || PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadVolume();
        }
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("sfx", volume);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 0.5f);
    }

}
