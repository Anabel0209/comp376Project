using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class HealthManagement : MonoBehaviour
{
    private int maxHealth = 5;
    public int healthPoints = 5;
    float timeInBetweenDamageLava = 2.0f;
    float damageTimer = 0f;
    float timeInBetweenDamageWater = 2.5f;
    bool inLava = false;
    bool inWater = false;
    
    public AudioSource hurtSound;
    Camera myCamera;

    //ui heart
    public GameObject[] myHearts;

    public Animator animator;

    private Vector3 bossAreaRespawnPosition = new Vector3(200, -33, 0); 
    private bool diedToBoss = false; 
    private Vector2 bossAreaMin = new Vector2(205, -35); 
    private Vector2 bossAreaMax = new Vector2(241, -25); 

    private bool checkpointActivated = false; 
    private Vector3 planet2SpawnPosition; 
    private Vector3 checkpointPosition; 


    public GameObject deathPanel;       
    public Button respawnButton;       
    private int currentPlanet;         
   



    private void Awake()
    {
        myCamera = Camera.main;
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

      
        if (respawnButton != null)
        {
            respawnButton.onClick.AddListener(RespawnPlayer);
        }

        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }

        planet2SpawnPosition = GameObject.Find("planet 2").transform.position;

        checkpointPosition = planet2SpawnPosition;

    }


    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        checkpointPosition = newCheckpoint;
        checkpointActivated = true;        
        Debug.Log("Checkpoint set at: " + checkpointPosition);
    }


    void Update()
    {
     
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
            TriggerDeath();
        }


        int newPlanet = (int)myCamera.GetComponent<cameraController>().planetnb;
        if (newPlanet != currentPlanet)
        {
            currentPlanet = newPlanet;
            Debug.Log("Planet transition detected. Restoring health...");
            RestoreHealthOnTravel();
        }

    }

 
    private void RestoreHealthOnTravel()
    {
        GainHealth(maxHealth); 
        Debug.Log("Health restored to maximum on planet transition.");
    }


    //method to call when the player takes damage
    public void TakeDamage(int damage)
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

    private void TriggerDeath()
    {
       
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
        }


        Time.timeScale = 0f;

        currentPlanet = (int)myCamera.GetComponent<cameraController>().planetnb;
        Debug.Log("Player died on planet: " + currentPlanet);




    }

    private void RespawnPlayer()
    {
        GetComponent<PlayerMovement>().enabled = false; 

        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }

        float currentPlanet = myCamera.GetComponent<cameraController>().planetnb;
        Vector3 respawnPosition;

     
        if (transform.position.x >= bossAreaMin.x && transform.position.x <= bossAreaMax.x &&
            transform.position.y >= bossAreaMin.y && transform.position.y <= bossAreaMax.y)
        {
            Debug.Log("Player died in boss area. Respawning at boss area coordinates.");
            respawnPosition = bossAreaRespawnPosition;
        }
        else if (currentPlanet == 2) 
        {
            if (checkpointActivated && transform.position.x >= checkpointPosition.x)
            {
                Debug.Log("Respawning at Planet 2's checkpoint.");
                respawnPosition = checkpointPosition; 
            }
            else
            {
                Debug.Log("Respawning at Planet 2's default spawn.");
                respawnPosition = planet2SpawnPosition; 
            }
        }
        else if (currentPlanet == 1) 
        {
            Debug.Log("Respawning at Planet 1's default spawn.");
            respawnPosition = GameObject.Find("planet 1").transform.position; 
        }
        else if (currentPlanet == 0) 
        {
            Debug.Log("Respawning at Main Planet's default spawn.");
            respawnPosition = GameObject.Find("main planet").transform.position; 
        }
        else
        {
            Debug.LogWarning("Invalid planet index. Respawning at Main Planet as fallback.");
            respawnPosition = GameObject.Find("main planet").transform.position; 
        }


        
        respawnPosition.z = 0f; 
        transform.position = respawnPosition;

        GainHealth(maxHealth);
        Time.timeScale = 1f;

        GetComponent<PlayerMovement>().enabled = true; 
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
