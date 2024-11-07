using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class HealthManagement : MonoBehaviour
{
    private int maxHealth = 5;
    public int healthPoints = 5;
    float damageTimer = 0f;
    float timeInBetweenDamage = 2.0f;
    bool inLava = false;
    public Transform respawnLocation;
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
            if (damageTimer >= timeInBetweenDamage)
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
        if(collision.collider.CompareTag("Spike"))
        {
            Debug.Log("Collided with Spike.");
            TakeDamage(1);
            ContactPoint2D contact = collision.contacts[0];
            Vector2 forceDirection = contact.normal;
            Debug.Log(forceDirection);
            gameObject.GetComponent<Rigidbody2D>().AddForce(forceDirection * 6f, ForceMode2D.Impulse);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Lava"))
        {
            inLava = false;
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