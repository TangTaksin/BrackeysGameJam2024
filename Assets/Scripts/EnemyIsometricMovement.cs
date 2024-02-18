using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyIsometricMovement : creature
{
    public float speed = 3f;
    public float attackRange = 1.5f;
    public int damage = 2;
    public float attackCooldown = 2f;
    public float sightRange = 5f;
    public LayerMask attackMask;
    float cdTimer;

    public float screamInterval = 20f;
    float screamTimer;

    public float movementDamping = 5f; // Adjust this value for smoother movement
    public player player;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isDead = false;

    bool isPaused = false;

    void Start()
    {
        if (player == null)
        {
            player = FindAnyObjectByType<player>();
        }

        InitializeComponents();
        VerifyComponents();

        cdTimer = 2;

        GameManager.OnPauseEvent += OnPause;
        GameManager.OnResumeEvent += OnResume;
    }

    void Update()
    {
        //raycheck
        var direction = (player.transform.position - transform.position).normalized;

        var hit = Physics2D.Raycast(transform.position, direction, sightRange, attackMask);

        if (!isDead && player != null && !isPaused && hit)
        {
            MoveTowardsPlayer();
        }
        else
        {
            StopChasing();
        }
    }

    void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void VerifyComponents()
    {
        if (spriteRenderer == null || animator == null || rb == null)
        {
            Debug.LogError("SpriteRenderer, Animator, or Rigidbody2D component not found on the enemy GameObject.");
        }
    }

    void MoveTowardsPlayer()
    {
        if (screamTimer <= 0)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.mon_walk_SFX);
            screamTimer = screamInterval;
        }

        screamTimer -= Time.deltaTime;

        Vector3 direction = (player.transform.position - transform.position).normalized;
        Vector2 movement = new Vector2(direction.x, direction.y);
        
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        // Smoothly adjust the velocity using damping
        rb.velocity = Vector2.Lerp(rb.velocity, movement * speed, Time.deltaTime * movementDamping);

        FlipSprite(movement.x);

        cdTimer -= Time.deltaTime;

        if ((distanceToPlayer <= attackRange * 2) && cdTimer <= 0)
        {
            PlayAttackAnimation();
            cdTimer = attackCooldown;
        }
        else
        {
            StopAttackAnimation();
        }
    }

    void StopChasing()
    {
        // Smoothly stop the monster by damping its velocity
        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, Time.deltaTime * movementDamping);
        StopAttackAnimation();
    }

    void FlipSprite(float directionX)
    {
        spriteRenderer.flipX = directionX <= 0;
    }


    void PlayAttackAnimation()
    {
        Debug.Log("Attacking Animation Playing");
        AudioManager.Instance?.PlaySFX(AudioManager.Instance.mon_attack_SFX);

        animator.SetBool("IsAttacking", true);
    }

    void StopAttackAnimation()
    {
        animator.SetBool("IsAttacking", false);
    }

    public void DamageTarget()
    {
        var hit = Physics2D.OverlapCircle(transform.position, attackRange, attackMask);

        print("attack! : " + hit?.name);

        if (hit)
            player.DamageHealth(damage);

        
    }


    void OnDrawGizmos()
    {
        DrawGizmo(Color.red, attackRange);
    }

    void DrawGizmo(Color color, float range)
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, range);
    }


    void OnPause()
    {
        isPaused = true;
    }

    void OnResume()
    {
        isPaused = false;
    }


    public override void OnHurt()
    {
        
    }

    public override void OnHealthZero()
    {
        Die();
    }

    public void Die()
    {
        isDead = true;
        animator.SetTrigger("IsDead");
        AudioManager.Instance.PlaySFX(AudioManager.Instance.mon_dead_SFX);
        rb.velocity = Vector2.zero;
        Destroy(gameObject, 1);
        // Additional death-related logic can be added here
    }
}
