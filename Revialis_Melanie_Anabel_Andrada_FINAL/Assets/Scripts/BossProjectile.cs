using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float knockbackForce = 10f; 
    public int damage = 1; 
    public float maxTravelDistance = 10f; 
    private Vector3 spawnPosition; 

    private void Start()
    {
        spawnPosition = transform.position; 
    }

    private void Update()
    {
       
        if (Vector3.Distance(spawnPosition, transform.position) >= maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            
            HealthManagement playerHealth = collision.collider.GetComponent<HealthManagement>();
            if (playerHealth != null)
            {
                Debug.Log("Projectile hit the player. Dealing damage...");
                playerHealth.TakeDamage(damage);
            }

           
            Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                playerRb.velocity = Vector2.zero; 
                playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }

      
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
