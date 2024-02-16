using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("==============Settings==============")]
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] public Slider musicSlider;
    [SerializeField] public Slider sfxSlider;

    void Awake()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
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
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        Debug.Log("Setting music volume to: " + volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        Debug.Log("Setting SFX volume to: " + volume);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void LoadVolume()
    {
        float storedMusicVolume = PlayerPrefs.GetFloat("musicVolume");
        float storedSFXVolume = PlayerPrefs.GetFloat("sfxVolume");
        musicSlider.value = storedMusicVolume;
        SetMusicVolume();
        sfxSlider.value = storedSFXVolume;
        SetSFXVolume();
    }

}
