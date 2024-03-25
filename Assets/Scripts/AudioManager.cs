using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSourcePrefab; // Prefab for SFX AudioSource

    [Header("Music Clips")]
    public AudioClip BGMusic;

    [Header("Player SFX Clips")]
    public AudioClip player_pick_ammo_SFX;
    public AudioClip pickMagSFX;
    public AudioClip playerWalkSFX;
    public AudioClip playerFireSFX;

    [Header("Door SFX Clips")]
    public AudioClip door_open_wood_sfx;
    public AudioClip door_close_wood_sfx;
    public AudioClip door_open_metal_sfx;
    public AudioClip door_close_metal_sfx;
    public AudioClip door_stuck_sfx;

    [Header("Monster SFX Clips")]
    public AudioClip mon_attack_SFX;
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
        if (musicSource != null && BGMusic != null)
        {
            PlayMusic(BGMusic);
        }
        else
        {
            Debug.LogError("musicSource or BGMusic is null");
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
        if (SFXClip != null)
        {
            // Instantiate a new AudioSource for the SFX
            AudioSource sfxSource = Instantiate(sfxSourcePrefab, transform.position, Quaternion.identity);
            sfxSource.PlayOneShot(SFXClip);
        }
        else
        {
            Debug.LogWarning("Trying to play null SFXClip");
        }
    }
}
