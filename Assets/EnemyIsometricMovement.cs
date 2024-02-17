using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyIsometricMovement : MonoBehaviour
{
    public float speed = 3f;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public Transform player;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isAttacking = false;
    private bool isDead = false;
    private Vector2 lastKnownDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (spriteRenderer == null || animator == null || rb == null)
        {
            Debug.LogError("SpriteRenderer, Animator, or Rigidbody2D component not found on the enemy GameObject.");
        }
    }

    void Update()
    {
        if (!isDead && !isAttacking && player != null)
        {
            CheckDetectionRange();
        }
        else
        {
            animator.SetBool("IsAttacking", false);
            isAttacking = false;
        }
    }

    void CheckDetectionRange()
    {
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            // Player is not detected, resume movement in the last known direction
            MoveInLastKnownDirection();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        lastKnownDirection = direction;

        Vector2 movement = new Vector2(direction.x, direction.y);
        rb.velocity = movement * speed;

        FlipSprite(movement.x);

        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            StartAttackAnimation();
        }
    }

    void MoveInLastKnownDirection()
    {
        // Move the enemy in the last known direction
        rb.velocity = lastKnownDirection * speed;

        // Flip the sprite based on the direction
        FlipSprite(lastKnownDirection.x);
    }

    void FlipSprite(float directionX)
    {
        spriteRenderer.flipX = directionX <= 0;
    }

    void StartAttackAnimation()
    {
        animator.SetBool("IsAttacking", true);
        isAttacking = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void Die()
    {
        isDead = true;
        animator.SetTrigger("IsDead");
        rb.velocity = Vector2.zero; // Stop movement on death
        Destroy(gameObject, 1);
        // Additional death-related logic can be added here
    }
}
