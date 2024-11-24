using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float knockbackForce = 10f; // Knockback force
    public int damage = 1; // Damage dealt to the player
    public float maxTravelDistance = 10f; // Maximum distance the projectile can travel
    private Vector3 spawnPosition; // Position where the projectile was spawned

    private void Start()
    {
        spawnPosition = transform.position; // Record the spawn position
    }

    private void Update()
    {
        // Check if the projectile has exceeded its travel distance
        if (Vector3.Distance(spawnPosition, transform.position) >= maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Deal damage to the player
            HealthManagement playerHealth = collision.collider.GetComponent<HealthManagement>();
            if (playerHealth != null)
            {
                Debug.Log("Projectile hit the player. Dealing damage...");
                playerHealth.TakeDamage(damage);
            }

            // Knockback the player
            Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                playerRb.velocity = Vector2.zero; // Reset current velocity
                playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }

        // Destroy the projectile on any collision
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        // Destroy the projectile when it goes out of the camera view
        Destroy(gameObject);
    }
}
