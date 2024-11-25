using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f; 
    public float moveDistance = 3f; 
    public float bounceForce = 5f; 
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.5f; 
    public AudioSource deathSound; 


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
        rb.gravityScale = 1; 
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; 

        if (deathSound == null)
        {
            Debug.LogWarning("Death sound is not assigned to the Enemy.");
        }

    }

    private void Update()
    {
       
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
              
                playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);
                PlayDeathSound();
                Destroy(gameObject);
            }
            else if (playerMovement != null && playerHealth != null && playerMovement.canMove)
            {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

              
                playerHealth.SendMessage("TakeDamage", 1);

               
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
            
            GameObject tempAudio = new GameObject("TempAudio");
            AudioSource tempAudioSource = tempAudio.AddComponent<AudioSource>();

            tempAudioSource.clip = deathSound.clip;
            tempAudioSource.volume = deathSound.volume;
            tempAudioSource.pitch = deathSound.pitch;
            tempAudioSource.loop = false;

            tempAudioSource.Play();
            Destroy(tempAudio, deathSound.clip.length);
        }
    }


}
