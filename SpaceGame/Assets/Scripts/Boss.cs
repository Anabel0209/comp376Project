using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int health = 5; // Number of jumps needed to kill the boss
    public float moveSpeed = 4f; // Speed of the boss
    public float moveRange = 5f; // Range of left and right movement
    public float knockbackForce = 10f; // Knockback force for the player
    public Transform player; // Reference to the player
    public GameObject projectilePrefab; // Projectile prefab
    public float projectileSpeed = 10f; // Speed of the projectile
    public float shootInterval = 2.3f; // Time between shots
    public GameObject cage;

    public AudioSource takeDamageSound; // Sound when boss takes damage
    public AudioSource deathSound; // Sound when boss dies
    public AudioSource shootSound; // Sound when boss shoots a projectile
    public AudioSource breathing; // Sound when the player is in range

    private Vector3 initialPosition;
    private bool movingRight = true;
    private Animator animator;
    private Rigidbody2D rb;

    // Boss area bounds
    private Vector2 bossAreaMin = new Vector2(205, -35); // Minimum bounds of the boss area
    private Vector2 bossAreaMax = new Vector2(241, -25); // Maximum bounds of the boss area

    private bool isDead = false;

    private bool isBreathingPlaying = false;

    public BoxCollider2D babyCollider;



    void Start()
    {
        initialPosition = transform.position;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Start shooting projectiles
     
        StartCoroutine(ShootProjectile());
    }

    void Update()
    {
        if (!isDead)
        {
            HandleMovement();
            CheckPlayerInBounds();
        }
        
    }

    private void HandleMovement()
    {
        // Handle left and right movement
        if (movingRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            if (transform.position.x > initialPosition.x + moveRange)
                movingRight = false;
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            if (transform.position.x < initialPosition.x - moveRange)
                movingRight = true;
        }
    }

    private void CheckPlayerInBounds()
    {
        if (player != null)
        {
            Vector2 playerPosition = player.position;

            // Check if the player is within the boss area bounds
            if (playerPosition.x >= bossAreaMin.x && playerPosition.x <= bossAreaMax.x &&
                playerPosition.y >= bossAreaMin.y && playerPosition.y <= bossAreaMax.y)
            {
                if (!isBreathingPlaying)
                {
                    if (breathing != null)
                    {
                        breathing.Play();
                        isBreathingPlaying = true;
                    }
                }
            }
            else
            {
                if (isBreathingPlaying)
                {
                    if (breathing != null)
                    {
                        breathing.Stop();
                        isBreathingPlaying = false;
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Collider2D bossCollider = GetComponent<Collider2D>();
            Vector2 contactPoint = collision.GetContact(0).point;
            float topThreshold = bossCollider.bounds.max.y - 0.1f;

            if (contactPoint.y > topThreshold)
            {
                TakeDamage();
                Vector2 knockbackDirection = (collision.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right;
                knockbackDirection += Vector2.up;
                knockbackDirection.Normalize();
                KnockbackPlayer(collision.collider, knockbackDirection);
            }
            else
            {
                DealDamageToPlayer(collision.collider);
                Vector2 knockbackDirection = (collision.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right;
                KnockbackPlayer(collision.collider, knockbackDirection);
            }
        }
    }

    private void KnockbackPlayer(Collider2D player, Vector2 direction)
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
            float strongKnockbackForce = knockbackForce * 1.5f;
            playerRb.AddForce(direction * strongKnockbackForce, ForceMode2D.Impulse);
        }
    }

    private void TakeDamage()
    {
        health--;

        if (animator != null)
        {
            // Set the TakeDamage trigger
            animator.SetTrigger("TakeDamage");
            Debug.Log("TakeDamage trigger activated");
        }

        if (takeDamageSound != null)
        {
            takeDamageSound.Play();
        }

        if (health <= 0)
        {
            Die();
        }
    }


    private void Die() { 

        if (isDead) return;

        isDead = true;

        
        if (deathSound != null)
        {
            deathSound.Play();
        }

        if (animator != null)
        {
            animator.Play("BossDeath"); // Replace "BossDeath" with the exact name of your animation state
            Debug.Log("Forcing BossDeath animation");
        }


        StopMovement();

        
        Destroy(cage);
        ReenableBabyCollider();
        StartCoroutine(WaitForDeathAnimation());
    }

    private void StopMovement()
{
    // Stop the movement logic
    movingRight = false; // Prevent movement logic from running
    rb.velocity = Vector2.zero; // Stop any existing velocity

    // Disable the Rigidbody2D if needed
    if (rb != null)
    {
        rb.simulated = false; // Stops all physics interactions
    }
}



    private IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private void DealDamageToPlayer(Collider2D player)
    {
        HealthManagement playerHealth = player.GetComponent<HealthManagement>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
        }
    }

    private void ReenableBabyCollider()
    {
        if (babyCollider != null)
        {
            babyCollider.enabled = true;
        }
        else
        {
            Debug.LogWarning("Baby's BoxCollider2D is not assigned.");
        }
    }


    private IEnumerator ShootProjectile()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(shootInterval);

            if (animator != null)
            {
                animator.SetTrigger("ShootTrigger");
            }

            if (projectilePrefab != null && !isDead)
            {
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

                Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
                Collider2D bossCollider = GetComponent<Collider2D>();

                if (projectileCollider != null && bossCollider != null)
                {
                    Physics2D.IgnoreCollision(projectileCollider, bossCollider);
                }

                Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
                if (projectileRb != null && player != null)
                {
                    Vector2 direction = (player.position - transform.position).normalized;
                    projectileRb.velocity = direction * projectileSpeed;

                    // Play shoot sound only if the player is within bounds
                    Vector2 playerPosition = player.position;
                    if (playerPosition.x >= bossAreaMin.x && playerPosition.x <= bossAreaMax.x &&
                        playerPosition.y >= bossAreaMin.y && playerPosition.y <= bossAreaMax.y)
                    {
                        if (shootSound != null)
                        {
                            shootSound.Play();
                        }
                    }
                }
            }
        }
    }
}
