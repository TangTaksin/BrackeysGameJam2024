using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class player : creature
{
    Rigidbody2D playerRb2D;
    Animator playerAnimator;
    Camera cam;

    Vector2 inputVector;
    Vector2 lateInput;

    bool isPause;

    [Header("direction strenght")]
    [SerializeField][Range(0,1)] float xStr = 1f;
    [SerializeField][Range(0, 1)] float yStr = 1f;

    [Header("Interaction")]
    [SerializeField] float interactCheckRadius = 1f;
    [SerializeField] LayerMask interactionLayer;

    List<Collider2D> interactList = new List<Collider2D>();
    Interactable currentInteractable;

    [SerializeField] RectTransform promptUI;
    Image promptImg;
    TextMeshProUGUI promptTxt;

    [Header("debug text")]
    [SerializeField] TextMeshProUGUI inputStrTxt;
    

    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnPauseEvent += OnPause;
        GameManager.OnResumeEvent += OnResume;

        playerRb2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        cam = Camera.main;

        promptImg = promptUI.GetComponentInChildren<Image>();
        promptTxt = promptUI.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPause)
        {
            GetInput();

            ApplyVelocity();

            InteractableCheck();
            Interact();

            Debug();
        }

        UpdateAnimator();
    }

    void GetInput()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");

        inputVector = new Vector2(x * xStr, y * yStr);
        inputVector.Normalize();
    }

    void UpdateAnimator()
    {
        //update last input
        lateInput = Vector2.Lerp(lateInput, inputVector, (int)inputVector.SqrMagnitude());

        playerAnimator.SetFloat("x", lateInput.x);
        playerAnimator.SetFloat("y", lateInput.y);
        playerAnimator.SetFloat("inputMagnitude", (int)inputVector.SqrMagnitude());

    }

    void ApplyVelocity()
    {
        playerRb2D.velocity = inputVector * moveSpeed;
    }

    void InteractableCheck()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, interactCheckRadius, interactionLayer);

        // if detect any interactables
        if (colliders.Length > 0)
        {
            //try raycasting to each one.
            foreach (var collider in colliders)
            {
                var cast = Physics2D.Raycast(transform.position, collider.transform.position - transform.position, Mathf.Infinity, interactionLayer);

                // if ray able to reach, add it to the list if it's not on it.
                if (cast && !interactList.Contains(collider))
                {
                    interactList.Add(collider);
                }
            }

            var closestDistance = Mathf.Infinity;

            //after that find highlight the closest one on the list
            foreach (var collider in interactList)
            {
                var newdistance = Vector3.Distance(transform.position, collider.transform.position);

                if (newdistance < closestDistance)
                {
                    closestDistance = newdistance;
                    currentInteractable?.DisableHighlight();
                    collider?.gameObject.TryGetComponent<Interactable>(out currentInteractable);
                }
            }

            currentInteractable?.ActiveHighlight();

        }
        else
        {
            currentInteractable?.DisableHighlight();
            currentInteractable = null;
        }

        //Prompt UI
        promptUI?.gameObject.SetActive(currentInteractable);

        if (currentInteractable && promptUI)
        {
            //move prompt to the interactable
            var newposition = cam.WorldToScreenPoint(currentInteractable.transform.position);
            promptUI.position = newposition;

            //update prompt info
            var info = currentInteractable.GiveInfo();
            promptImg.sprite = info.Item1;
            promptTxt.text = info.Item2;
        }

    }

    void Interact()
    {
        if (Input.GetButtonDown("Interact") && currentInteractable)
        {
            currentInteractable.Interact();
        }
    }

    void OnPause()
    {
        isPause = true;
    }

    void OnResume()
    {
        isPause = false;
    }

    void Debug()
    {
        if (inputStrTxt)
            inputStrTxt.text = string.Format("Input Magnitude : {0}", inputVector.SqrMagnitude().ToString());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactCheckRadius);
    }
}
