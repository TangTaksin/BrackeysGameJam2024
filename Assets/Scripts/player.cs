using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class player : creature
{
    Rigidbody2D playerRb2D;
    Animator playerAnimator;
    SpriteRenderer spriteRenderer;
    Camera cam;

    Vector2 inputVector;
    Vector2 lateInput;

    Vector2 dirToMouse;

    bool isPause;
    bool animLocked;

    [Header("direction strenght")]
    [SerializeField][Range(0, 1)] float xStr = 1f;
    [SerializeField][Range(0, 1)] float yStr = 1f;

    [Header("Interaction")]
    [SerializeField] float interactCheckRadius = 1f;
    [SerializeField] LayerMask interactionLayer;

    List<Collider2D> interactList = new List<Collider2D>();
    Interactable currentInteractable;

    [SerializeField] RectTransform promptUI;
    Image promptImg;
    TextMeshProUGUI promptTxt;

    [Header("Gunplay")]
    [SerializeField] Transform gunOrigin;
    [Space]
    [SerializeField] int damage = 2;
    [SerializeField] float coolDown = 1f;
    [SerializeField] int currentMagsize = 2;

    int currentBullet;

    [Space]
    [SerializeField] LayerMask gunMask;
    [Space]
    [SerializeField] Image cursorImage;
    [SerializeField] Sprite normal;
    [SerializeField] Sprite aimming;
    [SerializeField] Sprite reload;
    [Space]
    [SerializeField] LineRenderer laserSight;
    [SerializeField] float laserLenght;
    [Space]
    [SerializeField] Color shootColor;
    [SerializeField] Color reloadColor;
    [Space]

    Color cursorColor;

    bool isAimming;
    float cdTimer;

    [Header("debug text")]
    [SerializeField] TextMeshProUGUI inputStrTxt;

    [SerializeField] private FadeScreen fadeScreen;


    // Start is called before the first frame update
    void Start()
    {
        fadeScreen.GetComponent<FadeScreen>();
        GameManager.OnPauseEvent += OnPause;
        GameManager.OnResumeEvent += OnResume;

        playerRb2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        cam = Camera.main;

        promptImg = promptUI.GetComponentInChildren<Image>();
        promptTxt = promptUI.GetComponentInChildren<TextMeshProUGUI>();

        Cursor.visible = false;
        laserSight.enabled = false;

        currentBullet = currentMagsize; // set current bullut to mag size
    }

    // Update is called once per frame
    void Update()
    {
        GetMouseInput();

        if (!isPause)
        {
            GetInput();

            ApplyVelocity();

            InteractableCheck();
            Interact();

            Gunplay();

            DebugLog();
        }

        UpdateAnimator();
    }

    void GetInput()
    {
        // KeyBoard
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");

        inputVector = new Vector2(x * xStr, y * yStr);
        inputVector.Normalize();
    }

    void GetMouseInput()
    {
        // Mouse Position
        var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        if (!animLocked)
        {
            dirToMouse = mousePos - gunOrigin.position;
            dirToMouse.Normalize();
        }

        cursorImage.transform.position = Input.mousePosition;
    }


    void UpdateAnimator()
    {
        //update last input
        lateInput = Vector2.Lerp(lateInput, inputVector, (int)inputVector.SqrMagnitude());

        playerAnimator.SetFloat("x", lateInput.x);
        playerAnimator.SetFloat("y", lateInput.y);
        playerAnimator.SetFloat("inputMagnitude", (int)inputVector.SqrMagnitude());

        animLocked = playerAnimator.GetBool("AnimLock");

        if (!animLocked)
        {
            playerAnimator.SetFloat("aimX", dirToMouse.x);
            playerAnimator.SetFloat("aimY", dirToMouse.y);
        }
    }

    public void SetAnimationState(string stateName)
    {
        playerAnimator.Play(stateName);
    }

    public void HidePlayerVisual(bool value)
    {
        spriteRenderer.enabled = !value;
    }


    void ApplyVelocity()
    {
        playerRb2D.velocity = (inputVector * moveSpeed * (!animLocked).GetHashCode()) / (1 + (isAimming).GetHashCode());

        if (inputVector.SqrMagnitude() > 0)
        {
            //AudioManager.Instance.PlaySFX(AudioManager.Instance.playerWalkSFX);
        }
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
                if (cast && !interactList.Contains(collider) && collider.gameObject.activeSelf)
                {
                    interactList.Add(collider);
                }
            }

            var closestDistance = Mathf.Infinity;
            Interactable compare;

            //after that find highlight the closest one on the list
            foreach (var collider in interactList)
            {
                compare = collider?.gameObject.GetComponent<Interactable>();
                var newdistance = Vector3.Distance(gunOrigin.position, compare.GiveHighlightObject().transform.position);

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
            var newposition = cam.WorldToScreenPoint(currentInteractable.GiveHighlightObject().transform.position);
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


    void Gunplay()
    {
        if (Input.GetButtonDown("Aim"))
        {
            isAimming = true;
            playerAnimator.SetBool("aimming", isAimming);
            playerAnimator.Play("gun Pull Blend");

            cursorImage.sprite = aimming;
            laserSight.enabled = true;
        }

        if (Input.GetButtonUp("Aim"))
        {
            isAimming = false;
            playerAnimator.SetBool("aimming", isAimming);

            cursorImage.sprite = normal;
            laserSight.enabled = false;
        }

        if (isAimming)
        {
            if (Input.GetButtonUp("Shoot") && cdTimer <= 0 && currentBullet > 0)
            {
                cdTimer = coolDown;
                playerAnimator.SetTrigger("Shot");

                AddBullentCount(-1);

                AudioManager.Instance?.PlaySFX(AudioManager.Instance.playerFireSFX);

                var hit = Physics2D.Raycast(gunOrigin.position, dirToMouse, Mathf.Infinity, gunMask);

                if (hit)
                {
                    hit.collider.GetComponent<creature>().DamageHealth(damage);
                }

            }
        }

        //gun cooldown
        cdTimer -= Time.deltaTime;
        if (cdTimer <= 0)
        {
            cdTimer = 0;
            if (isAimming)
                cursorImage.sprite = aimming;
        }

        // cursor

        if (cdTimer > 0 && isAimming)
        {
            cursorImage.sprite = reload;
        }

        if (currentBullet <= 0)
        {
            cursorColor = reloadColor;
        }
        else
        {
            cursorColor = Color.Lerp(shootColor, reloadColor, cdTimer / coolDown);
        }

        cursorImage.color = cursorColor;

        laserSight.SetPosition(0, gunOrigin.position);
        laserSight.SetPosition(1, (Vector2)gunOrigin.position + dirToMouse * laserLenght);

        laserSight.startColor = new Color(cursorColor.r, cursorColor.g, cursorColor.b, 1f);
        laserSight.endColor = new Color(cursorColor.r, cursorColor.g, cursorColor.b, 0);

    }

    public void AddMagSize(int amount)
    {
        currentMagsize += amount;
    }

    public void AddBullentCount(int amount)
    {
        currentBullet += amount;

        if (currentBullet > currentMagsize)
            currentBullet = currentMagsize;
    }


    Vector2 storedVelo;

    void OnPause()
    {
        isPause = true;
        if (playerRb2D != null)
        {
            storedVelo = playerRb2D.velocity; // store
            playerRb2D.velocity = Vector2.zero;

        }


    }

    void OnResume()
    {
        isPause = false;

        if (playerRb2D != null)
        {
            playerRb2D.velocity = storedVelo;
        }

    }

    public override void OnHealthZero()
    {
        playerAnimator.Play("player_ded");
        GameManager.RequestPause();
        fadeScreen.GameOver();

    }


    void DebugLog()
    {
        if (inputStrTxt)
            inputStrTxt.text = string.Format("Bullet : {0}", currentBullet);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactCheckRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(gunOrigin.position, gunOrigin.position + (Vector3)dirToMouse * 5);
    }

}
