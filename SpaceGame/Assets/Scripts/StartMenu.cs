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
    public AudioSource backgroundAudio;   // Reference to the AudioSource component for background music
    public RectTransform scrollingText;   // Reference to the scrolling text's RectTransform

    public GameObject helpButton;         // Reference to the Help Button GameObject
    public GameObject helpPanel;          // Reference to the Help Panel GameObject
    public GameObject closeButton;        // Reference to the Close Button on the help panel

    public float fadeDuration = 2f;       // Duration of the fade effect
    public float scrollSpeed = 75f;       // Speed at which the scrolling text moves
    public float scrollEndY = 1310f;       // The Y-position at which the scrolling stops (adjust as needed)

    private CanvasGroup fadeCanvasGroup;

    private void Start()
    {
        // Ensure the start button and welcome text are hidden initially
        startButton.SetActive(false);
        welcomeText.SetActive(false);
        helpButton.SetActive(false);
        helpPanel.SetActive(false);

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

        // Set up the Close Button click event if assigned
        if (closeButton != null)
        {
            Button closeBtn = closeButton.GetComponent<Button>();
            if (closeBtn != null)
            {
                closeBtn.onClick.AddListener(CloseHelpPanel);
            }
        }
    }

    private void OnIntroVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Intro Video Ended");
        // Start fading to black when the intro video ends
        StartCoroutine(FadeToBlackAndShowScrollingText());
    }

    private IEnumerator FadeToBlackAndShowScrollingText()
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

        // Fade back to transparent before showing the scrolling text
        Debug.Log("Fading to Transparent Before Scrolling Text");
        yield return StartCoroutine(FadeCanvasGroup(fadeCanvasGroup, 1f, 0f, fadeDuration));
        Debug.Log("Fade to Transparent Complete");

        // Start scrolling text
        scrollingText.gameObject.SetActive(true);
        yield return StartCoroutine(ScrollText());

        // Hide the scrolling text when done
        scrollingText.gameObject.SetActive(false);

        // Show welcome text after scrolling text finishes
        welcomeText.SetActive(true);

        // Wait for 1 second before showing the start button
        yield return new WaitForSeconds(1f);

        // Show the start button after the delay
        startButton.SetActive(true);
        helpButton.SetActive(true);
        Debug.Log("Button Activated");
    }


    private IEnumerator ScrollText()
    {
        Vector3 startPosition = scrollingText.anchoredPosition;
        Vector3 targetPosition = new Vector3(startPosition.x, scrollEndY, startPosition.z);

        while (scrollingText.anchoredPosition.y < scrollEndY)
        {
            scrollingText.anchoredPosition = Vector3.MoveTowards(scrollingText.anchoredPosition, targetPosition, scrollSpeed * Time.deltaTime);
            yield return null;
        }
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
        SceneManager.LoadScene("Mission");
    }

    public void ShowHelpPanel()
    {
        if (helpPanel != null)
        {
            helpPanel.SetActive(true);
        }
    }

    public void CloseHelpPanel()
    {
        if (helpPanel != null)
        {
            helpPanel.SetActive(false);
        }
    }
}
