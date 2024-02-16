using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("===================Audio Source===================")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("===================Music Clips===================")]
    public AudioClip BGMusic;
    

    [Header("===================Player SFX Clips===================")]
    public AudioClip player_pick_ammo_SFX;
    public AudioClip pickMagSFX;
    public AudioClip playerWalkSFX;
    public AudioClip playerFireSFX;

    [Header("===================Player SFX Clips===================")]
    public AudioClip door_open_wood_sfx;
    public AudioClip door_close_wood_sfx;
    public AudioClip door_open_metal_sfx;
    public AudioClip door_close_metal_sfx;
    public AudioClip door_stuck_sfx;

    [Header("===================Monster SFX Clips===================")]
    public AudioClip mon_dead_SFX;
    public AudioClip mon_walk_SFX;



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
