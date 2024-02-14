using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("===================Audio Source===================")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("===================Audio Clips===================")]
    public AudioClip BGMusic;
    public AudioClip pickUpSFX;


    private void Start()
    {
        PlayMusic(BGMusic);

    }

    public void PlayMusic(AudioClip musicClip)
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(AudioClip SFXClip)
    {
        SFXSource.PlayOneShot(SFXClip);
    }

}
