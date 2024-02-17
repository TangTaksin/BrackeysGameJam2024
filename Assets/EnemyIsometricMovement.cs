using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyIsometricMovement : MonoBehaviour
{
    public float speed = 3f;
    public float attackRange = 1.5f;
    public float movementDamping = 5f; // Adjust this value for smoother movement
    public Transform player;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        InitializeComponents();
        VerifyComponents();
    }

    void Update()
    {
        if (!isDead && player != null)
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
        Vector3 direction = (player.position - transform.position).normalized;
        Vector2 movement = new Vector2(direction.x, direction.y);

        // Smoothly adjust the velocity using damping
        rb.velocity = Vector2.Lerp(rb.velocity, movement * speed, Time.deltaTime * movementDamping);

        FlipSprite(movement.x);

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= attackRange * 2)
        {
            PlayAttackAnimation();
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
        animator.SetBool("IsAttacking", true);
    }

    void StopAttackAnimation()
    {
        animator.SetBool("IsAttacking", false);
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

    public void Die()
    {
        isDead = true;
        animator.SetTrigger("IsDead");
        rb.velocity = Vector2.zero;
        Destroy(gameObject, 1);
        // Additional death-related logic can be added here
    }
}
