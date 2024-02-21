using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class door : Interactable
{
    [Header("Next Scene")]
    [SerializeField] string connectedSceneName;

    bool isPeeked;

    [Header("Lock Option")]
    [SerializeField] bool isLocked;

    SpriteRenderer doorRenderer;
    Animator doorSpriteAnimator;
    
    AudioSource audioSource;

    [Header("Audio")]
    [SerializeField] AudioClip peekAudio;
    [SerializeField] AudioClip openAudio, closeAudio, stuckAudio;

    [Header("Visual")]
    [SerializeField] GameObject positionA;
    [SerializeField] GameObject positionB;
    [SerializeField] Animator shadowAnimator;

    player playerObj; //ref

    public delegate void doorEvent(door doorData);
    public doorEvent OnPeeked;
    public doorEvent OnMoveaway;
    public static doorEvent OnEnter;

    private void Start()
    {
        doorRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        doorSpriteAnimator = GetComponent<Animator>();

        playerObj = FindAnyObjectByType<player>();
        positionA.SetActive(false);
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

                doorSpriteAnimator.SetTrigger("peek");
                shadowAnimator.SetTrigger("peek");

                isPeeked = true;
            }

            OnPeeked?.Invoke(this);

            // base.Interact() show "enter or not" prompt
            base.Interact();

            //teleport player to A, and hide player visual 
            playerObj.HidePlayerVisual(true);
            positionA.SetActive(true);

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
        base.OnYes();

        SceneManager.LoadScene(connectedSceneName);

        GameManager.RequestResume();
        doorSpriteAnimator.SetTrigger("enter");
        shadowAnimator.SetTrigger("enter");

        playerObj.HidePlayerVisual(false);
        positionA.SetActive(false);

        isLocked = true;
    }

    // don't, if no
    protected override void OnNo()
    {
        base.OnNo();

        OnMoveaway?.Invoke(this);

        playerObj.HidePlayerVisual(false);
        positionA.SetActive(false);

        GameManager.RequestResume();
    }

}
