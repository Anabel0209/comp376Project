using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MissionManager : MonoBehaviour
{
    public GameObject fadePanel; // Panel for fade effect
    public float fadeDuration = 4f; // Duration for fade in/out
    public GameObject landButton; // Reference to the Land button
    private CanvasGroup fadeCanvasGroup;


    private float lastClickTime = 0f;
    private float clickDelay = 2f;

    public DialogueManager dialogueManager; // Reference to the DialogueManager for starting the President's dialogue

    private Dialogue presidentDialogue; // Reference to the President's Dialogue component
    private bool hasDialogueStarted = false; // Flag to ensure dialogue starts once
    private bool hasDialogueEnded = false;

    private bool isProcessing = false;

    void Start()
    {
        // Find the President's Dialogue component
        GameObject presidentGameObject = GameObject.Find("President");
        if (presidentGameObject != null)
        {
            presidentDialogue = presidentGameObject.GetComponent<Dialogue>();
        }

        // Initialize and hide the Land button
        if (landButton != null)
        {
            landButton.SetActive(false);
        }

        // Fade in effect
        fadeCanvasGroup = fadePanel.GetComponent<CanvasGroup>();
        if (fadeCanvasGroup != null && !hasDialogueStarted)
        {
            StartCoroutine(FadeInAndStartDialogue());
        }
    }

    

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastClickTime < clickDelay) return; // Prevent rapid clicks
            lastClickTime = Time.time;
        }
    }


    IEnumerator FadeInAndStartDialogue()
    {

        if (isProcessing) yield break; // Prevent additional triggers
        isProcessing = true;


        float elapsedTime = 0f;
        fadeCanvasGroup.alpha = 1f; // Start fully opaque

        

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = 1f - (elapsedTime / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f; // Fully transparent
        fadePanel.SetActive(false); // Optional: disable panel if not needed after fade

        // Start the dialogue with the President after the fade, only if it hasn't started
        if (dialogueManager != null && presidentDialogue != null && !hasDialogueStarted)
        {
            hasDialogueStarted = true;
            dialogueManager.StartDialogue(presidentDialogue.lines, presidentDialogue.textSpeed, presidentDialogue.playerLines, presidentDialogue.npcName);
        }

        // Listen for the completion of the dialogue
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.OnDialogueEnd += OnDialogueEnd;
        }
    }

    private void OnDialogueEnd()
    {
        // Display the Land button when dialogue ends
        if (landButton != null)
        {
            hasDialogueEnded = true;
            landButton.SetActive(true);
        }
    }

    public void OnLandButtonClicked()
    {
        // Load the Level_1 scene
        SceneManager.LoadScene("Level_1");
    }

    private void OnDestroy()
    {
        // Unsubscribe from the dialogue end event to avoid memory leaks
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.OnDialogueEnd -= OnDialogueEnd;
        }
    }
}
