using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class PresidentDialogueManager : MonoBehaviour
{
    public static PresidentDialogueManager instance; 
    public TextMeshProUGUI npcTextComponent;
    public TextMeshProUGUI playerTextComponent;
    public GameObject dialoguePanel;
    public GameObject playerPanel;
    public TextMeshProUGUI npcNameTextComponent;

    public AudioSource npcAudioSource;
    public AudioSource playerAudioSource;  

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
            StopAllCoroutines(); 
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
        isTyping = true; 

        npcTextComponent.text = string.Empty;

        foreach (string line in lines)
        {
            npcTextComponent.text = string.Empty; 
                                                  
            if (npcAudioSource != null && !npcAudioSource.isPlaying)
            {
                npcAudioSource.Play();
            }

            foreach (char c in line)
            {
                npcTextComponent.text += c;
                yield return new WaitForSeconds(textSpeed);
            }

            
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        isTyping = false;
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
        isTyping = true; 
        playerPanel.SetActive(true);
        playerTextComponent.text = string.Empty;

        while (playerLinesQueue.Count > 0)
        {
            string line = playerLinesQueue.Dequeue();
            playerTextComponent.text = string.Empty;

         
            if (playerAudioSource != null && !playerAudioSource.isPlaying)
            {
                playerAudioSource.Play();
            }

            foreach (char c in line)
            {
                playerTextComponent.text += c;
                yield return new WaitForSeconds(0.05f);
            }

           
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        isTyping = false; 
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
