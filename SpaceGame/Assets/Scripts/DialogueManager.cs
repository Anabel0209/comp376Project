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

        // Debug to check if npcName is received correctly
        Debug.Log("NPC Name received: " + npcName);

        // Set the NPC's name
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
    }
}
