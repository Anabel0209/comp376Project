using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance; // Singleton instance

    public TextMeshProUGUI npcTextComponent;
    public TextMeshProUGUI playerTextComponent; // Text component for player lines
    public GameObject dialoguePanel;
    public GameObject playerPanel; // Panel to show when the player is speaking
    public TextMeshProUGUI npcNameTextComponent;

    private NPCExclamationMark currentNpcExclamation;


   // public GameObject endGamePanel; // Reference to the end-game panel
    //public Button endGameResumeButton; // Resume button on the end-game panel
    //public Button endGameQuitButton; // Quit button on the end-game panel
   // public GameObject mayorEndGame;


    public AudioSource playerSound; 

    public PlayerMovement playerMovement; // Reference to PlayerMovement script

    private Coroutine typingCoroutine;
    private Queue<string> playerLinesQueue; // Queue to hold player lines

    private bool isDialogueActive = false; // Flag to track active dialogue state
    private bool isTyping = false; // Flag to control typing state
    public event System.Action OnDialogueEnd;
    private bool isInputLocked = false;


    private bool endGamePanelAllowed = false;


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
       // if (endGamePanel != null) endGamePanel.SetActive(false);

        // Set up button listeners
        //if (endGameResumeButton != null) endGameResumeButton.onClick.AddListener(ResumeGame);
        //if (endGameQuitButton != null) endGameQuitButton.onClick.AddListener(QuitGame);
    }

    public void StartDialogue(string[] npcLines, float textSpeed, string[] playerLines = null, string npcName = "", AudioSource npcAudioSource = null)
    {
        Debug.Log($"StartDialogue called. NPC Name: {npcName}");
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
        Debug.Log("EndDialogue called.");
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

        // Check if this is the mayorEndGame
        //if (mayorEndGame != null && mayorEndGame.activeInHierarchy)
        //{
        //    Debug.Log("Dialogue with mayorEndGame complete. Opening End-Game Panel.");
        //    ShowEndGamePanel();
        //}
        //else
        //{
        //    Debug.Log("Dialogue with non-mayor NPC complete. No panel will open.");
        //}

    }

    //private void ShowEndGamePanel()
    //{
       // Debug.Log("ShowEndGamePanel called.");
       // if (!mayorEndGame.activeInHierarchy)
       // {
       //     Debug.LogError("ShowEndGamePanel() called incorrectly. Preventing activation.");
        //    return;
       //}

        //if (endGamePanel != null)
       // {
        //    Debug.Log("Displaying End-Game Panel.");
        //    endGamePanel.SetActive(true);
        //    Time.timeScale = 0f; // Pause the game
        //}
    //}

    //private void ResumeGame()
    //{
     //   if (endGamePanel != null) endGamePanel.SetActive(false);
    //    Time.timeScale = 1f; // Resume the game
    //    endGamePanelAllowed = false;
    //}

    //private void QuitGame()
   // {
    //    Debug.Log("Quitting game...");

        // This will quit the application when running outside of the Unity Editor
   //     Application.Quit();

        // This ensures you can see it working in the Unity Editor
//#if UNITY_EDITOR
//    UnityEditor.EditorApplication.isPlaying = false;
//#endif
  //  }




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