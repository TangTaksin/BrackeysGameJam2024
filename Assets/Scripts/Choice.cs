using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Choice : MonoBehaviour
{
    [Tooltip("If unassigned, this will be defaulted to its attached gameobject")][SerializeField] GameObject choiceObj;

    [Header("Required to be set")]
    [SerializeField] TextMeshProUGUI choiceQuestionTxt;

    public delegate void CallEvent(string question);
    public static CallEvent CallChoiceEvent;

    public delegate void ChoiceEvent();
    public static ChoiceEvent yesEvent;
    public static ChoiceEvent noEvent;

    private void Start()
    {
        CallChoiceEvent += CallChoice;

        if (!choiceObj)
        {
            choiceObj = gameObject;
        }

        choiceObj.SetActive(false);
    }

    public void CallChoice(string question)
    {
        //GameObject
        GameManager.RequestPause();
        choiceQuestionTxt.text = question;
        choiceObj.SetActive(true);
    }

    public void PutChoiceAway()
    {
        choiceObj.SetActive(false);
    }

    private void Update()
    {
        //Wait for input if Choice is active
        if (choiceObj.activeSelf)
        {
            if (Input.GetButtonDown("Confirm"))
            {
                yesEvent?.Invoke();
                PutChoiceAway();
            }
            if (Input.GetButtonDown("Cancel"))
            {
                noEvent?.Invoke();
                PutChoiceAway();
            }
        }

    }
}
