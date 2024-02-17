using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineUpgrade : Interactable
{
    [SerializeField] int UpgradeAmount = 2;
    [SerializeField] AudioClip pickUpAudio;

    AudioSource auidoSauce;

    player player;

    private void Start()
    {
        player = FindAnyObjectByType<player>();
        auidoSauce = GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        player.AddMagSize(UpgradeAmount);
        player.AddBullentCount(UpgradeAmount);

        AudioManager.Instance.PlaySFX(AudioManager.Instance.pickMagSFX);

        gameObject.SetActive(false);
        base.Interact();
    }
}
