using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemCollectionManager : MonoBehaviour
{
    public TMP_Text counterItems;
    public int goal;
    public int count;
    public Sprite itemToCollect;
    public bool shouldCollectItem = true;
    public GameObject itemCollectionDisplay;
    public GameObject inGameImageOfItem;
    private bool hasReachedGoal;
    public AudioSource coinSound;
    public int totalNbGem;
    public GameObject endGameGrid;
    public GameObject endGameDecoration;
    
    public AudioSource allCoinsAlarm;
    public AudioSource rumble;
    public CameraShake cameraShake;
    private bool hasInteracted = false;
    private bool endGameSequencePlayed = false;


    private AudioManager audioManager;

    private cameraController camController; // Reference to check the current planet

    public GameObject arrow; // Reference to the arrow GameObject
    public GameObject spaceship; // Reference to the spaceship GameObject




    private void Start()
    {
        // Find AudioManager in the scene
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager script is missing in the scene!");
        }

        // Start the arrow flashing effect
        if (arrow != null)
        {
            StartCoroutine(FlashArrow());
        }

    }


    private void Update()
    {
        

        inGameImageOfItem.GetComponent<Image>().sprite = itemToCollect;
        if (shouldCollectItem)
        {
            counterItems.SetText(count.ToString());
            itemCollectionDisplay.SetActive(true);
            if (goal == count)
            {
                hasReachedGoal = true;
                //itemCollectionDisplay.SetActive(false);
                //goal = 0;
                //count = 0;
            }
        }
        else
        {
            //set visible the main planet upgrade
            itemCollectionDisplay.SetActive(false);

            
        }

        //if the player collected all the gems
        if(count == totalNbGem && endGameSequencePlayed == false)
        {
            //so that the sequence happen only once
            endGameSequencePlayed = true;
            
            //play a sounds
            //allCoinsAlarm.Play();
            rumble.Play();

            //shake the screen
            StartCoroutine(cameraShake.Shake(1.5f, 0.4f));

            //player say something
            if (hasInteracted) return; // Prevent multiple interactions
            hasInteracted = true;

            
            DialogueManager.instance.StartDialogue(
             new string[0], // No NPC lines
             0.05f,         // Text speed
             new string[] { "What was that?" }, // Player's line
             "Turnip hat");

            //enable the modifications on the main planet
            endGameGrid.SetActive(true);
            endGameDecoration.SetActive(true);

            // Trigger audio manager to switch to final main planet audio
            if (audioManager != null)
            {
                audioManager.ActivateFinalAudio();
            }

            // Hide arrow and spaceship
            HideArrowAndSpaceship();

            // Strictly ensure `endGamePanel` never opens during gem collection
            //if (DialogueManager.instance != null && DialogueManager.instance.endGamePanel != null)
            //{
            //  if (DialogueManager.instance.endGamePanel.activeSelf)
            // {
            //     Debug.LogError("Gem collection attempted to trigger End-Game Panel. Forcing closure.");
            //     DialogueManager.instance.endGamePanel.SetActive(false); // Force close
            // }
            //}
        }
    }


    private void HideArrowAndSpaceship()
    {
        if (arrow != null)
        {
            arrow.SetActive(false);
        }

        if (spaceship != null)
        {
            spaceship.SetActive(false);
        }

        Debug.Log("Arrow and spaceship hidden at end game.");
    }

    private IEnumerator FlashArrow()
    {
        while (!endGameSequencePlayed) // Flash until end game is reached
        {
            if (arrow != null)
            {
                arrow.SetActive(!arrow.activeSelf); // Toggle visibility
            }
            yield return new WaitForSeconds(0.5f); // Adjust flash speed as needed
        }
    }


    public void DecrementCount(int amountToDecrement)
    {
        if (count >= amountToDecrement)
        {
            return;
           // count = count - amountToDecrement;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectible"))
        {

            //coinSound = collision.gameObject.GetComponent<AudioSource>();
            coinSound.Play();
            count++;
            Destroy(collision.gameObject);

        }
    }
}