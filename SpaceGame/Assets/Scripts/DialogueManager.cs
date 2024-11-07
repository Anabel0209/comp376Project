using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance; // Singleton instance

    public TextMeshProUGUI npcTextComponent;
    public TextMeshProUGUI playerTextComponent; // Text component for player lines
    public GameObject dialoguePanel;
    public GameObject playerPanel; // Panel to show when the player is speaking
    public TextMeshProUGUI npcNameTextComponent;

    public AudioSource alienSound; // Audio source for the alien sound effect
    public AudioSource playerSound; // Audio source for the player sound effect

    public PlayerMovement playerMovement; // Reference to PlayerMovement script

    private Coroutine typingCoroutine;
    private Queue<string> playerLinesQueue; // Queue to hold player lines

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        playerLinesQueue = new Queue<string>();

        // Ensure panels are hidden at the start
        dialoguePanel.SetActive(false);
        playerPanel.SetActive(false);
    }

    public void StartDialogue(string[] npcLines, float textSpeed, string[] playerLines = null, string npcName = "")
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Disable player movement at the start of dialogue
        if (playerMovement != null)
        {
            playerMovement.DisableMovement();
        }

        npcNameTextComponent.text = npcName;
        typingCoroutine = StartCoroutine(TypeLines(npcLines, textSpeed));

        if (playerLines != null)
        {
            playerLinesQueue.Clear();
            foreach (string line in playerLines)
            {
                playerLinesQueue.Enqueue(line);
            }
        }
    }

    IEnumerator TypeLines(string[] lines, float textSpeed)
    {
        dialoguePanel.SetActive(true);
        npcTextComponent.text = string.Empty;
        playerPanel.SetActive(false); // Ensure player panel is hidden during NPC dialogue

        foreach (string line in lines)
        {
            npcTextComponent.text = string.Empty;
            PlayAlienSound(); // Play alien sound when NPC starts a line

            foreach (char c in line.ToCharArray())
            {
                npcTextComponent.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
            // Wait for user input before moving to the next line
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        DisplayPlayerLines();
    }

    void DisplayPlayerLines()
    {
        if (playerLinesQueue.Count > 0)
        {
            StartCoroutine(TypePlayerLines());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypePlayerLines()
    {
        playerPanel.SetActive(true); // Show player panel when the player is speaking
        playerTextComponent.text = string.Empty;

        while (playerLinesQueue.Count > 0)
        {
            string line = playerLinesQueue.Dequeue();
            playerTextComponent.text = string.Empty;

            // Play the player sound once when starting a new player line
            PlayPlayerSound();


            foreach (char c in line.ToCharArray())
            {
                playerTextComponent.text += c;
                yield return new WaitForSeconds(0.05f); // Adjust the text speed as needed
            }
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        playerPanel.SetActive(false); // Hide the panel after the player's lines are done
        EndDialogue();
    }

    public void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        dialoguePanel.SetActive(false);
        npcTextComponent.text = string.Empty;
        playerTextComponent.text = string.Empty;

        // Enable player movement after the dialogue ends
        if (playerMovement != null)
        {
            playerMovement.EnableMovement();
        }
    }

    private void PlayAlienSound()
    {
        if (alienSound != null)
        {
            if (alienSound.isPlaying)
            {
                alienSound.Stop(); // Stop the current sound if it's playing
            }
            alienSound.Play();
        }
    }

    private void PlayPlayerSound()
    {
        if (playerSound != null)
        {
            if (playerSound.isPlaying)
            {
                playerSound.Stop(); // Stop the current sound if it's playing
            }
            playerSound.Play();
        }
    }
}