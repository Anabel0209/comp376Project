using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class StartMenu : MonoBehaviour
{
    public VideoPlayer introVideoPlayer;  // Reference to the Intro Video Player
    public VideoPlayer backgroundVideoPlayer;  // Reference to the Background Looping Video Player
    public GameObject startButton;        // Reference to the Start Button GameObject
    public GameObject welcomeText;        // Reference to the Welcome Text UI element
    public GameObject fadePanel;          // Reference to the Fade Panel for transitions
    public float fadeDuration = 2f;       // Duration of the fade effect

    public AudioSource backgroundAudio;   // Reference to the AudioSource component for background music

    private CanvasGroup fadeCanvasGroup;

    private void Start()
    {
        // Ensure the start button and welcome text are hidden initially
        startButton.SetActive(false);
        welcomeText.SetActive(false);

        // Get or add the CanvasGroup component for fading
        fadeCanvasGroup = fadePanel.GetComponent<CanvasGroup>();
        if (fadeCanvasGroup == null)
        {
            fadeCanvasGroup = fadePanel.AddComponent<CanvasGroup>();
        }
        fadeCanvasGroup.alpha = 0f; // Start fully transparent


        // Play background audio if assigned
        if (backgroundAudio != null)
        {
            backgroundAudio.loop = true; // Ensure looping
            backgroundAudio.Play();
        }


        // Register for the video completion event for the intro video
        if (introVideoPlayer != null)
        {
            introVideoPlayer.loopPointReached += OnIntroVideoEnd;
        }

        // Ensure the background video is inactive initially
        if (backgroundVideoPlayer != null)
        {
            backgroundVideoPlayer.gameObject.SetActive(false);
        }

        // Start playing the intro video
        if (introVideoPlayer != null && !introVideoPlayer.isPlaying)
        {
            introVideoPlayer.Play();
        }
    }

    private void OnIntroVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Intro Video Ended");
        // Start fading to black when the intro video ends
        StartCoroutine(FadeToBlackAndShowText());
    }

    private IEnumerator FadeToBlackAndShowText()
    {
        Debug.Log("Starting Fade to Black");

        // Fade the panel to black (opaque)
        yield return StartCoroutine(FadeCanvasGroup(fadeCanvasGroup, 0f, 1f, fadeDuration));
        Debug.Log("Fade to Black Complete");

        // Hide the intro video and activate the background video
        if (introVideoPlayer != null)
        {
            introVideoPlayer.gameObject.SetActive(false);
        }
        if (backgroundVideoPlayer != null)
        {
            backgroundVideoPlayer.gameObject.SetActive(true);
            backgroundVideoPlayer.isLooping = true;
            backgroundVideoPlayer.Play();
        }

        // Brief pause for visual effect (optional)
        yield return new WaitForSeconds(0.5f);

        // Show welcome text and fade back to transparent (fade out the black overlay)
        welcomeText.SetActive(true);
        Debug.Log("Fading to Transparent");
        yield return StartCoroutine(FadeCanvasGroup(fadeCanvasGroup, 1f, 0f, fadeDuration));
        Debug.Log("Fade to Transparent Complete");

        // Wait for 1 second before showing the start button
        yield return new WaitForSeconds(1f);

        // Show the start button after the delay
        startButton.SetActive(true);
        Debug.Log("Button Activated");
    }






    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        Debug.Log($"Starting fade from {startAlpha} to {endAlpha}");
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            canvasGroup.alpha = newAlpha;
            Debug.Log($"Current Alpha: {canvasGroup.alpha}");
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
        Debug.Log("Fade completed");
    }


    public void StartGame()
    {
        Debug.Log("Start Game Button Clicked");
        // Load the Level_1 scene when the button is clicked
        SceneManager.LoadScene("Level_1");
    }
}
