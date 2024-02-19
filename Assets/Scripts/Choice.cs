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

    player player;
    Camera cam;

    public delegate void CallEvent(string question);
    public static CallEvent CallChoiceEvent;

    public delegate void ChoiceEvent();
    public static ChoiceEvent yesEvent;
    public static ChoiceEvent noEvent;

    private void Start()
    {
        player = FindAnyObjectByType<player>();
        cam = Camera.main;

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
        Debug.Log("Choice activated: " + question);
    }

    public void PutChoiceAway()
    {
        choiceObj.SetActive(false);
    }

    private void Update()
    {
        // Wait for input if Choice is active
        if (choiceObj != null && choiceObj.activeSelf)
        {
            // position
            var tar = cam.WorldToScreenPoint(player.transform.position);
            choiceObj.transform.position = Vector3.Lerp(choiceObj.transform.position, tar, .1f);

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

    private void OnDestroy()
    {
        CallChoiceEvent -= CallChoice;
        yesEvent = null;
        noEvent = null;
    }
}
