using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance; 

    public TextMeshProUGUI npcTextComponent;
    public TextMeshProUGUI playerTextComponent; 
    public GameObject dialoguePanel;
    public GameObject playerPanel; 
    public TextMeshProUGUI npcNameTextComponent;

    private NPCExclamationMark currentNpcExclamation;



    public AudioSource playerSound; 

    public PlayerMovement playerMovement; 

    private Coroutine typingCoroutine;
    private Queue<string> playerLinesQueue; 

    private bool isDialogueActive = false; 
    private bool isTyping = false; 
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

       
        dialoguePanel.SetActive(false);
        playerPanel.SetActive(false);
       // if (endGamePanel != null) endGamePanel.SetActive(false);

        //if (endGameResumeButton != null) endGameResumeButton.onClick.AddListener(ResumeGame);
        //if (endGameQuitButton != null) endGameQuitButton.onClick.AddListener(QuitGame);
    }

    public void StartDialogue(string[] npcLines, float textSpeed, string[] playerLines = null, string npcName = "", AudioSource npcAudioSource = null)
    {
        Debug.Log($"StartDialogue called. NPC Name: {npcName}");
        if (isDialogueActive || IsInputLocked()) return; 

        LockInput();
        isDialogueActive = true;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }


     
        currentNpcExclamation = FindObjectOfType<NPCExclamationMark>(); 
        if (currentNpcExclamation != null)
        {
            currentNpcExclamation.StopFlashing();
        }


       
        if (playerMovement != null)
        {
            playerMovement.DisableMovement();
        }


        
        if (npcName == "Turnip hat")
        {
            npcNameTextComponent.text = ""; 
            if (playerLines != null)
            {
                playerLinesQueue.Clear();
                foreach (string line in playerLines)
                {
                    playerLinesQueue.Enqueue(line);
                }
            }
            DisplayPlayerLines(); 
            return;
        }

     
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
                npcAudioSource.Stop();
            }
            npcAudioSource.Play(); 
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
        playerPanel.SetActive(false);

        foreach (string line in lines)
        {
                
        PlayNpcSound(npcAudioSource);

            if (isTyping) yield break; 

            isTyping = true; 
            npcTextComponent.text = string.Empty;
            

            foreach (char c in line)
            {
                npcTextComponent.text += c;
                yield return new WaitForSeconds(textSpeed);
            }

            isTyping = false; 

           
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
        playerPanel.SetActive(true); 
        playerTextComponent.text = string.Empty;

        while (playerLinesQueue.Count > 0)
        {
            string line = playerLinesQueue.Dequeue();
            playerTextComponent.text = string.Empty;

           
            PlayPlayerSound();


            foreach (char c in line.ToCharArray())
            {
                playerTextComponent.text += c;
                yield return new WaitForSeconds(0.05f); 
            }
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        playerPanel.SetActive(false); 
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

      
        if (currentNpcExclamation != null)
        {
            currentNpcExclamation.StopFlashing();
            currentNpcExclamation = null;
        }


     
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
                playerSound.Stop(); 
            }
            playerSound.Play();
        }
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}