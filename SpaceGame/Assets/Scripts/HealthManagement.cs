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

    private Vector3 bossAreaRespawnPosition = new Vector3(200, -33, 0); // Coordinates for respawning after boss death
    private bool diedToBoss = false; // Flag to track if the player died due to the boss
    private Vector2 bossAreaMin = new Vector2(205, -35); // Minimum bounds of the boss area
    private Vector2 bossAreaMax = new Vector2(241, -25); // Maximum bounds of the boss area

    private bool checkpointActivated = false; // Flag for checkpoint activation
    private Vector3 planet2SpawnPosition; // Default spawn position for Planet 2
    private Vector3 checkpointPosition; // The position of the active checkpoint



    // Death panel
    public GameObject deathPanel;       // Panel that appears when the player dies
    public Button respawnButton;       // Respawn button on the death panel
    private int currentPlanet;         // Track the current planet the player is on
   



    private void Awake()
    {
        myCamera = Camera.main;
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Set up the respawn button listener
        if (respawnButton != null)
        {
            respawnButton.onClick.AddListener(RespawnPlayer);
        }

        // Ensure the death panel is hidden initially
        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }

        // Set the default spawn position for Planet 2 (adjust coordinates as needed)
        planet2SpawnPosition = GameObject.Find("planet 2").transform.position;

        // Initialize checkpoint position to default Planet 2 spawn
        checkpointPosition = planet2SpawnPosition;

    }


    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        checkpointPosition = newCheckpoint; // Update checkpoint position
        checkpointActivated = true;        // Activate checkpoint logic
        Debug.Log("Checkpoint set at: " + checkpointPosition);
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
            TriggerDeath();
        }

        // Restore health on planet transition
        int newPlanet = (int)myCamera.GetComponent<cameraController>().planetnb;
        if (newPlanet != currentPlanet)
        {
            currentPlanet = newPlanet;
            Debug.Log("Planet transition detected. Restoring health...");
            RestoreHealthOnTravel();
        }

    }

    // New method to restore health on planet transitions
    private void RestoreHealthOnTravel()
    {
        GainHealth(maxHealth); // Restore health to maximum
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


    // Trigger the death sequence
    private void TriggerDeath()
    {
        // Stop player movement and show the death panel
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
        }

        // Pause the game
        Time.timeScale = 0f;

        // Save the current planet the player died on
        currentPlanet = (int)myCamera.GetComponent<cameraController>().planetnb;
        Debug.Log("Player died on planet: " + currentPlanet);




    }

    private void RespawnPlayer()
    {
        GetComponent<PlayerMovement>().enabled = false; // Disable movement temporarily

        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }

        // Get the current planet from the cameraController
        float currentPlanet = myCamera.GetComponent<cameraController>().planetnb;
        Vector3 respawnPosition;

        // Check if the player died in the boss area
        if (transform.position.x >= bossAreaMin.x && transform.position.x <= bossAreaMax.x &&
            transform.position.y >= bossAreaMin.y && transform.position.y <= bossAreaMax.y)
        {
            Debug.Log("Player died in boss area. Respawning at boss area coordinates.");
            respawnPosition = bossAreaRespawnPosition;
        }
        else if (currentPlanet == 2) // Planet 2 logic
        {
            if (checkpointActivated && transform.position.x >= checkpointPosition.x)
            {
                Debug.Log("Respawning at Planet 2's checkpoint.");
                respawnPosition = checkpointPosition; // Respawn at checkpoint
            }
            else
            {
                Debug.Log("Respawning at Planet 2's default spawn.");
                respawnPosition = planet2SpawnPosition; // Default spawn for Planet 2
            }
        }
        else if (currentPlanet == 1) // Planet 1 logic
        {
            Debug.Log("Respawning at Planet 1's default spawn.");
            respawnPosition = GameObject.Find("planet 1").transform.position; // Default spawn for Planet 1
        }
        else if (currentPlanet == 0) // Main Planet logic
        {
            Debug.Log("Respawning at Main Planet's default spawn.");
            respawnPosition = GameObject.Find("main planet").transform.position; // Default spawn for Main Planet
        }
        else
        {
            Debug.LogWarning("Invalid planet index. Respawning at Main Planet as fallback.");
            respawnPosition = GameObject.Find("main planet").transform.position; // Fallback to Main Planet spawn
        }


        // Ensure the correct z position
        respawnPosition.z = 0f; // Set the player's z position to 0 (or the desired value)
        transform.position = respawnPosition;

        // Restore health and re-enable movement
        GainHealth(maxHealth);
        Time.timeScale = 1f;

        GetComponent<PlayerMovement>().enabled = true; // Re-enable movement
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
