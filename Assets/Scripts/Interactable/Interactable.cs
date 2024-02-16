using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    [SerializeField] Sprite promptImage;
    [SerializeField][Multiline] string promptInfo;
    [SerializeField] GameObject highLight;

    [Header("Choices (Optional)")]
    [SerializeField] bool hasChoice;
    [SerializeField] bool pauseWhileChoice;
    [SerializeField][Multiline] protected string choiceInfo;

    public virtual void Interact()
    {
        if (hasChoice)
        {
            print("has choice");

            Choice.CallChoiceEvent?.Invoke(choiceInfo);
            Choice.yesEvent += OnYes;
            Choice.noEvent += OnNo;

            if (pauseWhileChoice)
            {
                GameManager.RequestPause();
            }
        }
    }

    public (Sprite, string) GiveInfo()
    {
        return (promptImage, promptInfo);
    }

    public virtual void ActiveHighlight()
    {
        highLight?.SetActive(true);
    }

    public virtual void DisableHighlight()
    {
        highLight?.SetActive(false);
    }

    protected virtual void OnYes()
    {
        RemoveChoiceListeners();
    }

    protected virtual void OnNo()
    {
        RemoveChoiceListeners();
    }

    protected void RemoveChoiceListeners()
    {
        Choice.yesEvent -= OnYes;
        Choice.noEvent -= OnNo;
        GameManager.RequestResume();
    }
}