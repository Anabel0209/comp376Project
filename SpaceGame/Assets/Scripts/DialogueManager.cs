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

    private NPCExclamationMark currentNpcExclamation;


    public AudioSource playerSound; 

    public PlayerMovement playerMovement; // Reference to PlayerMovement script

    private Coroutine typingCoroutine;
    private Queue<string> playerLinesQueue; // Queue to hold player lines

    private bool isDialogueActive = false; // Flag to track active dialogue state
    private bool isTyping = false; // Flag to control typing state
    public event System.Action OnDialogueEnd;
    private bool isInputLocked = false;

 



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

    public void StartDialogue(string[] npcLines, float textSpeed, string[] playerLines = null, string npcName = "", AudioSource npcAudioSource = null)
    {
        if (isDialogueActive || IsInputLocked()) return; // Prevent restarting dialogue

        LockInput();
        isDialogueActive = true;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }


        // Stop exclamation mark flashing on the NPC
        currentNpcExclamation = FindObjectOfType<NPCExclamationMark>(); // Assign the active NPC's exclamation mark
        if (currentNpcExclamation != null)
        {
            currentNpcExclamation.StopFlashing();
        }


        // Disable player movement at the start of dialogue
        if (playerMovement != null)
        {
            playerMovement.DisableMovement();
        }


        // Skip NPC lines for "Turnip hat" and play only player lines
        if (npcName == "Turnip hat")
        {
            npcNameTextComponent.text = ""; // No NPC name for Turnip hat
            if (playerLines != null)
            {
                playerLinesQueue.Clear();
                foreach (string line in playerLines)
                {
                    playerLinesQueue.Enqueue(line);
                }
            }
            DisplayPlayerLines(); // Immediately start player lines
            return;
        }

        // Regular dialogue behavior for other NPCs
        npcNameTextComponent.text = npcName;
        typingCoroutine = StartCoroutine(TypeLines(npcLines, textSpeed, npcAudioSource));

        if (playerLines != null)
        {
            playerLinesQueue.Clear();
            foreach (string line in playerLines)
            {
                playerLinesQueue.Enqueue(line);
            }
        }
    }

    private void PlayNpcSound(AudioSource npcAudioSource)
    {
        if (npcAudioSource != null)
        {
            if (npcAudioSource.isPlaying)
            {
                npcAudioSource.Stop(); // Stop any current sound
            }
            npcAudioSource.Play(); // Play the new sound
        }
    }


    IEnumerator TypeLines(string[] lines, float textSpeed, AudioSource npcAudioSource)
    {

        if (gameObject.name == "Turnip hat")
        {
            dialoguePanel.SetActive(false);
            DisplayPlayerLines();
        }
        else { 
        dialoguePanel.SetActive(true);
        npcTextComponent.text = string.Empty;
        playerPanel.SetActive(false); // Ensure player panel is hidden during NPC dialogue

        foreach (string line in lines)
        {
                // Play the NPC-specific sound
        PlayNpcSound(npcAudioSource);

            if (isTyping) yield break; // Prevent overlapping typing

            isTyping = true; // Set typing flag
            npcTextComponent.text = string.Empty;
            

            foreach (char c in line)
            {
                npcTextComponent.text += c;
                yield return new WaitForSeconds(textSpeed);
            }

            isTyping = false; // Reset typing flag

            // Wait for user input before moving to the next line
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        DisplayPlayerLines();
        }
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

        // Stop exclamation mark flashing for the current NPC
        if (currentNpcExclamation != null)
        {
            currentNpcExclamation.StopFlashing();
            currentNpcExclamation = null; // Clear the reference after stopping
        }


        // Enable player movement after the dialogue ends
        if (playerMovement != null)
        {
            playerMovement.EnableMovement();
        }

        isDialogueActive = false;
        UnlockInput();
        OnDialogueEnd?.Invoke();
    }

    private void LockInput()
    {
        isInputLocked = true;
    }

    private void UnlockInput()
    {
        isInputLocked = false;
    }

    public bool IsInputLocked()
    {
        return isInputLocked;
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

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}