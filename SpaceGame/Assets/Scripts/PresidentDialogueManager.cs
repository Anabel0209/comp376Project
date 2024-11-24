using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic; // Required for Queue<T>


public class PresidentDialogueManager : MonoBehaviour
{
    public static PresidentDialogueManager instance; // Singleton for President's dialogue
    public TextMeshProUGUI npcTextComponent;
    public TextMeshProUGUI playerTextComponent;
    public GameObject dialoguePanel;
    public GameObject playerPanel;
    public TextMeshProUGUI npcNameTextComponent;

    public AudioSource npcAudioSource;  // Audio source for NPC dialogue
    public AudioSource playerAudioSource;  // Audio source for Player dialogue

    private Queue<string> playerLinesQueue;
    private bool isTyping = false;

    private bool isDialogueActive = false;

    public event System.Action OnDialogueEnd;


    private void Awake()
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
        dialoguePanel.SetActive(false);
        playerPanel.SetActive(false);
    }

    public void StartDialogue(string[] npcLines, float textSpeed, string[] playerLines, string npcName)
    {

        if (isDialogueActive)
        {
            Debug.LogWarning("Dialogue already in progress!");
            return;
        }

        Debug.Log("Starting dialogue...");
        isDialogueActive = true;


        if (isTyping)
        {
            StopAllCoroutines(); // Stop any ongoing coroutines to prevent overlap
            isTyping = false;
        }

        dialoguePanel.SetActive(true);
        npcNameTextComponent.text = npcName;

        StartCoroutine(TypeLines(npcLines, textSpeed));

        if (playerLines != null)
        {
            playerLinesQueue.Clear();
            foreach (string line in playerLines)
            {
                playerLinesQueue.Enqueue(line);
            }
        }
    }


    private IEnumerator TypeLines(string[] lines, float textSpeed)
    {
        isTyping = true; // Set typing flag to true

        npcTextComponent.text = string.Empty;

        foreach (string line in lines)
        {
            npcTextComponent.text = string.Empty; // Clear the text for the new line
                                                  // Play NPC audio
            if (npcAudioSource != null && !npcAudioSource.isPlaying)
            {
                npcAudioSource.Play();
            }

            foreach (char c in line)
            {
                npcTextComponent.text += c;
                yield return new WaitForSeconds(textSpeed);
            }

            // Wait for user input before proceeding to the next line
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        isTyping = false; // Reset typing flag
        DisplayPlayerLines();
    }


    private void DisplayPlayerLines()
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

    private IEnumerator TypePlayerLines()
    {
        isTyping = true; // Set typing flag
        playerPanel.SetActive(true);
        playerTextComponent.text = string.Empty;

        while (playerLinesQueue.Count > 0)
        {
            string line = playerLinesQueue.Dequeue();
            playerTextComponent.text = string.Empty;

            // Play Player audio
            if (playerAudioSource != null && !playerAudioSource.isPlaying)
            {
                playerAudioSource.Play();
            }

            foreach (char c in line)
            {
                playerTextComponent.text += c;
                yield return new WaitForSeconds(0.05f);
            }

            // Wait for user input to proceed to the next line
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        isTyping = false; // Reset typing flag
        playerPanel.SetActive(false);
        EndDialogue();
    }


    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        npcTextComponent.text = string.Empty;
        playerTextComponent.text = string.Empty;

        isDialogueActive = false;
        OnDialogueEnd?.Invoke();
    }
}
