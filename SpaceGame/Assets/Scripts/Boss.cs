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


    public AudioSource takeDamageSound; // Sound when boss takes damage
    public AudioSource deathSound; // Sound when boss dies
    public AudioSource shootSound; // Sound when boss shoots a projectile


    private Vector3 initialPosition;
    private bool movingRight = true;
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        initialPosition = transform.position;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Test manual instantiation
        Debug.Log("Testing projectile instantiation...");
        if (projectilePrefab != null)
        {
            GameObject testProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Debug.Log("Projectile manually instantiated: " + testProjectile.name);
        }
        else
        {
            Debug.LogError("Projectile Prefab is not assigned!");
        }

        // Start shooting projectiles
        StartCoroutine(ShootProjectile());
    }

    void Update()
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Get the bounds of the boss's collider
            Collider2D bossCollider = GetComponent<Collider2D>();
            Vector2 contactPoint = collision.GetContact(0).point;
            float topThreshold = bossCollider.bounds.max.y - 0.1f; // Adjust to fine-tune the "top hit" area

            // Check if the player hit the top of the boss
            if (contactPoint.y > topThreshold)
            {
                Debug.Log("Player hit the top of the boss. Dealing damage to the boss.");
                TakeDamage(); // Damage the boss

                // Calculate knockback direction based on player's position relative to the boss
                Vector2 knockbackDirection = (collision.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right;
                knockbackDirection += Vector2.up; // Add an upward component for a diagonal knockback
                knockbackDirection.Normalize(); // Normalize to ensure consistent strength

                KnockbackPlayer(collision.collider, knockbackDirection); // Apply knockback
            }
            else
            {
                Debug.Log("Player hit the side or bottom of the boss. Damaging the player.");
                DealDamageToPlayer(collision.collider); // Damage the player

                // Determine knockback direction (left or right) based on player's position
                Vector2 knockbackDirection = (collision.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right;
                KnockbackPlayer(collision.collider, knockbackDirection); // Apply knockback
            }
        }
    }

    private void KnockbackPlayer(Collider2D player, Vector2 direction)
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero; // Reset current velocity
            float strongKnockbackForce = knockbackForce * 2f; // Increase knockback force
            playerRb.AddForce(direction * strongKnockbackForce, ForceMode2D.Impulse);
            Debug.Log($"Player knocked back with force: {direction * strongKnockbackForce}");
        }
    }





    private void TakeDamage()
    {
        health--;

        if (takeDamageSound != null)
        {
            takeDamageSound.Play();
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

        if (deathSound != null)
        {
            deathSound.Play();
        }

        // Handle boss death (e.g., play animation, destroy object)
        Destroy(gameObject);
    }

    private void DealDamageToPlayer(Collider2D player)
    {
        HealthManagement playerHealth = player.GetComponent<HealthManagement>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1); // Use your `HealthManagement` script's `TakeDamage` method
        }
    }

  


    private IEnumerator ShootProjectile()
{
    Debug.Log("ShootProjectile coroutine started.");

    while (true)
    {
        yield return new WaitForSeconds(shootInterval);

        // Play shoot animation
        if (animator != null)
        {
            Debug.Log("Playing shoot animation.");
            animator.SetTrigger("ShootTrigger");
        }

        // Instantiate the projectile
        if (projectilePrefab != null)
        {
            Debug.Log("Attempting to instantiate projectile...");
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Debug.Log("Projectile instantiated successfully: " + projectile.name);


            // Ignore collision between boss and projectile
            Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
            Collider2D bossCollider = GetComponent<Collider2D>();

            // Prevent immediate collision with the boss
            if (projectileCollider != null && bossCollider != null)
            {
                Physics2D.IgnoreCollision(projectileCollider, bossCollider);
            }

            // Apply velocity to the projectile
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            if (projectileRb != null && player != null)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                projectileRb.velocity = direction * projectileSpeed;
                Debug.Log("Projectile velocity set: " + projectileRb.velocity);

                if (shootSound != null)
                {
                    shootSound.Play();
                }

                }
            else
            {
                Debug.LogError("Projectile Rigidbody2D or Player Transform is missing!");
            }
        }
        else
        {
            Debug.LogError("Projectile Prefab is not assigned!");
        }
    }
}

}
