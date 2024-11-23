using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class HealthManagement : MonoBehaviour
{
    private int maxHealth = 5;
    public int healthPoints = 5;
    float timeInBetweenDamageLava = 2.0f;
    float damageTimer = 0f;
    float timeInBetweenDamageWater = 1.5f;
    bool inLava = false;
    bool inWater = false;
    public Transform respawnLocation;
    public AudioSource hurtSound;
    Camera myCamera;

    //ui heart
    public GameObject[] myHearts;

    public Animator animator;

    private void Awake()
    {
        myCamera = Camera.main;
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        //take damage one uppon entering the collider then every 2 seconds while staying in it
        if (inLava)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= timeInBetweenDamageLava)
            {
                TakeDamage(1);
                damageTimer = 0f;
                Debug.Log(healthPoints);
            }
        }
        //take damage every 2 seconds while in water
        if(inWater)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= timeInBetweenDamageWater)
            {
                TakeDamage(1);
                damageTimer = 0f;
                Debug.Log(healthPoints);
            }
        }
        if(healthPoints == 0)
        {
            Respawn();
        }

    }
    //method to call when the player takes damage
    private void TakeDamage(int damage)
    {
        //play hurt sound
        hurtSound.Play();

        if (healthPoints > 0)
        {
            Debug.Log("TakeDamage method called, triggering animation.");
            if (animator != null)
            {
                animator.SetTrigger("TakeDamage");
                Debug.Log("Animator TakeDamage trigger set.");
            }

            for (int i = 0; i < damage; i++)
            {
                healthPoints -= damage;
                myHearts[healthPoints].SetActive(false);
            }
        }
        else
        {
            Debug.Log(healthPoints + ": Player dead");
        }
    }




    private void GainHealth(int healthGain)
    {
        if(healthPoints < maxHealth)
        {
            for(int i = 0; i < healthGain; i++)
            {
                myHearts[healthPoints].SetActive(true);
                healthPoints++;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Lava"))
        {
            Debug.Log("Collided with Lava");
            TakeDamage(1);
            inLava = true;

        }
        if(collision.collider.CompareTag("water"))
        {
            Debug.Log("Collided with Water");
            inWater = true;
        }
        if(collision.collider.CompareTag("VerticalSpike"))
        {
            gameObject.GetComponent<PlayerMovement>().canMove = false;

            Debug.Log("Collided with Spike.");
            TakeDamage(1);
            ContactPoint2D contact = collision.contacts[0];
            Vector2 forceDirection = contact.normal;
            Debug.Log(forceDirection);
            gameObject.GetComponent<Rigidbody2D>().AddForce(forceDirection * 4f, ForceMode2D.Impulse);

        }
        if (collision.collider.CompareTag("HorizontalSpike"))
        {
            Debug.Log("Collided with Spike.");
            TakeDamage(1);
            ContactPoint2D contact = collision.contacts[0];
            Vector2 forceDirection = contact.normal;
            Debug.Log(forceDirection);
            gameObject.GetComponent<Rigidbody2D>().AddForce(forceDirection * 6f, ForceMode2D.Impulse);
        }
        if (collision.collider.CompareTag("Slide"))
        {
            Debug.Log("Collided with slide.");
            gameObject.GetComponent<PlayerMovement>().StartSliding();

        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Lava"))
        {
            inLava = false;
        }
        if(collision.collider.CompareTag("water"))
        {
            inWater = false;
        }
    }
    private void Respawn()
    {
        myCamera.GetComponent<cameraController>().planetnb = 0;
        transform.position = new Vector2(respawnLocation.position.x, respawnLocation.position.y);
        GainHealth(maxHealth);
    }
    public void ResetTakeDamage()
    {
        if (animator != null)
        {
            animator.ResetTrigger("TakeDamage");
            Debug.Log("TakeDamage trigger reset.");
        }
    }


}
