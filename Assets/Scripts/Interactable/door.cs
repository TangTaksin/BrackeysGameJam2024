using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : Interactable
{
    bool isPeeked;
    [Header("Lock")]
    [SerializeField] bool isLocked;

    SpriteRenderer doorRenderer;

    AudioSource audioSource;

    [Header("Audio")]
    [SerializeField] AudioClip peekAudio;
    [SerializeField] AudioClip openAudio, closeAudio, stuckAudio;

    [Header("Player Position")]
    [SerializeField] GameObject positionA;
    [SerializeField] GameObject positionB;

    player playerObj; //ref

    public delegate void doorEvent();
    public static doorEvent OnPeeked;
    public static doorEvent OnMoveaway;

    private void Start()
    {
        doorRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        playerObj = FindAnyObjectByType<player>();
    }

    public void SetIsPeeked(bool val)
    {
        isPeeked = val;
    }

    public void SetIsLocked(bool val)
    {
        isLocked = val;
    }

    public override void Interact()
    {
        if (!isLocked)
        {
            // if it hasn't been peeked yet.
            if (!isPeeked)
            {
                PlayPeekSound();

                // start door peeking animation
                OnPeeked?.Invoke();
                isPeeked = true;
            }

            // base.Interact() show "enter or not" prompt
            base.Interact();

            //teleport player to A
            playerObj.transform.position = positionA.transform.position;
        }
        else
        {
            PlayStuckAudio();
        }
    }

    public void SetVisibility(bool value)
    {
        if (value)
        {
            doorRenderer.color = Color.white;
        }
        else
        {
            doorRenderer.color = Color.clear;
        }
    }


    public void PlayPeekSound()
    {
        audioSource.PlayOneShot(peekAudio);
    }

    public void PlayOpenSound()
    {
        audioSource.PlayOneShot(openAudio);
    }

    public void PlayCloseSound()
    {
        audioSource.PlayOneShot(closeAudio);
    }

    public void PlayStuckAudio()
    {
        audioSource.PlayOneShot(stuckAudio);
    }


    // Enter next room if yes
    protected override void OnYes()
    {
        //teleport to B
        playerObj.transform.position = positionB.transform.position;
        GameManager.RequestResume();
        isLocked = true;
    }

    // don't, if no
    protected override void OnNo()
    {
        GameManager.RequestResume();
    }
}
