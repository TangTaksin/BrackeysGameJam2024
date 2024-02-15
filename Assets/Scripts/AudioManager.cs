using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("===================Audio Source===================")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("===================Audio Clips===================")]
    public AudioClip BGMusic;
    public AudioClip pickUpSFX;

    public static AudioManager Instance;
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


    private void Start()
    {
        if (musicSource != null)
        {
            PlayMusic(BGMusic);
        }
        else
        {
            Debug.LogError("musicSource is null");
        }

    }

    public void PlayMusic(AudioClip musicClip)
    {
        if (musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.Play();
        }

    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(AudioClip SFXClip)
    {
        sfxSource.PlayOneShot(SFXClip);
    }

}
