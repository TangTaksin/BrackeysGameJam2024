using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : Interactable
{
    [SerializeField] int BullutAmount = 5;

    player player;

    private void Start()
    {
        player = FindAnyObjectByType<player>();
    }

    public override void Interact()
    {
        player.AddBullentCount(BullutAmount);

        AudioManager.Instance.PlaySFX(AudioManager.Instance.player_pick_ammo_SFX);

        gameObject.SetActive(false);
        base.Interact();
    }
}
