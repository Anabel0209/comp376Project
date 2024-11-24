using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of the enemy's movement
    public float moveDistance = 3f; // Distance the enemy moves left and right
    public float bounceForce = 5f; // Force applied to the player when they defeat an enemy
    public float knockbackForce = 5f; // Force applied to the player when hit by an enemy
    public float knockbackDuration = 0.5f; // Duration of knockback effect
    public AudioSource deathSound; // AudioSource for the death sound

    private Vector2 startingPosition;
    private bool movingRight = true;
    private Rigidbody2D rb;

    private void Start()
    {
        startingPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 1; // Set a gravity scale (adjust as needed)
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent the enemy from rotating

        if (deathSound == null)
        {
            Debug.LogWarning("Death sound is not assigned to the Enemy.");
        }

    }

    private void Update()
    {
        // Move the enemy left and right within the specified range
        if (movingRight)
        {
            transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);

            if (transform.position.x >= startingPosition.x + moveDistance)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);

            if (transform.position.x <= startingPosition.x - moveDistance)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            HealthManagement playerHealth = collision.gameObject.GetComponent<HealthManagement>();

            if (playerRb != null && collision.contacts[0].normal.y < -0.5f)
            {
                // Player jumps on top of the enemy
                playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);
                PlayDeathSound();
                Destroy(gameObject);
            }
            else if (playerMovement != null && playerHealth != null && playerMovement.canMove)
            {
                // Player collides with the enemy from the side or bottom
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

                // Reduce player health
                playerHealth.SendMessage("TakeDamage", 1);

                // Disable player movement temporarily
                StartCoroutine(DisablePlayerMovementTemporarily(playerMovement));
            }
        }
    }

    private IEnumerator DisablePlayerMovementTemporarily(PlayerMovement playerMovement)
    {
        playerMovement.DisableMovement();
        yield return new WaitForSeconds(knockbackDuration);
        playerMovement.EnableMovement();
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void PlayDeathSound()
    {
        if (deathSound != null)
        {
            deathSound.Play(); // Play the death sound
        }
    }

}
