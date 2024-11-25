using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MissionManager : MonoBehaviour
{
    public GameObject fadePanel;
    public float fadeDuration = 4f; 
    public GameObject landButton; 
    private CanvasGroup fadeCanvasGroup;


    public AudioSource backgroundMusic; 

    private float lastClickTime = 0f;
    private float clickDelay = 2f;

    private PresidentDialogueManager dialogueManager;
    private PresidentDialogue presidentDialogue;


    private bool hasDialogueStarted = false; 
    private bool hasDialogueEnded = false;

    private bool isProcessing = false;

    void Start()
    {

    
        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true; 
            backgroundMusic.Play();
        }

        dialogueManager = FindObjectOfType<PresidentDialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("PresidentDialogueManager not found in the scene!");
            return;
        }

        GameObject presidentGameObject = GameObject.Find("President");
        if (presidentGameObject != null)
        {
            presidentDialogue = presidentGameObject.GetComponent<PresidentDialogue>();
        }


        if (landButton != null)
        {
            landButton.SetActive(false);
        }


        fadeCanvasGroup = fadePanel.GetComponent<CanvasGroup>();
        if (fadeCanvasGroup != null && !hasDialogueStarted)
        {
            StartCoroutine(FadeInAndStartDialogue());
        }

        dialogueManager.OnDialogueEnd += OnDialogueEnd;

    }

    

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastClickTime < clickDelay) return; 
            lastClickTime = Time.time;
        }
    }


    IEnumerator FadeInAndStartDialogue()
    {

        if (isProcessing) yield break; 
        isProcessing = true;


        float elapsedTime = 0f;
        fadeCanvasGroup.alpha = 1f; 

        

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = 1f - (elapsedTime / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f; 
        fadePanel.SetActive(false); 

       
        if (dialogueManager != null && presidentDialogue != null && !hasDialogueStarted)
        {
            hasDialogueStarted = true;
            dialogueManager.StartDialogue(presidentDialogue.lines, presidentDialogue.textSpeed, presidentDialogue.playerLines, presidentDialogue.npcName);
        }

        if (PresidentDialogueManager.instance != null)
        {
            PresidentDialogueManager.instance.OnDialogueEnd += OnDialogueEnd;
        }
    }

    private void OnDialogueEnd()
    {
        if (landButton != null)
        {
            hasDialogueEnded = true;
            landButton.SetActive(true);
        }
    }

    public void OnLandButtonClicked()
    {
        SceneManager.LoadScene("Level_1");
    }

    private void OnDestroy()
    {
        if (dialogueManager != null)
        {
            dialogueManager.OnDialogueEnd -= OnDialogueEnd;
        }
    }
}
