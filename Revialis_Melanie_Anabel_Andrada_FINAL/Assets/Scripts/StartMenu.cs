using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class StartMenu : MonoBehaviour
{
    public VideoPlayer introVideoPlayer;  
    public VideoPlayer backgroundVideoPlayer;  
    public GameObject startButton;        
    public GameObject welcomeText;        
    public GameObject fadePanel;         
    public AudioSource backgroundAudio;   
    public RectTransform scrollingText;  

    public GameObject helpButton;        
    public GameObject helpPanel;         
    public GameObject closeButton;        

    public float fadeDuration = 2f;      
    public float scrollSpeed = 75f;      
    public float scrollEndY = 1310f;      

    private CanvasGroup fadeCanvasGroup;

    private void Start()
    {
     
        startButton.SetActive(false);
        welcomeText.SetActive(false);
        helpButton.SetActive(false);
        helpPanel.SetActive(false);

        fadeCanvasGroup = fadePanel.GetComponent<CanvasGroup>();
        if (fadeCanvasGroup == null)
        {
            fadeCanvasGroup = fadePanel.AddComponent<CanvasGroup>();
        }
        fadeCanvasGroup.alpha = 0f; 

 
        if (backgroundAudio != null)
        {
            backgroundAudio.loop = true; 
            backgroundAudio.Play();
        }

        if (introVideoPlayer != null)
        {
            introVideoPlayer.loopPointReached += OnIntroVideoEnd;
        }

        if (backgroundVideoPlayer != null)
        {
            backgroundVideoPlayer.gameObject.SetActive(false);
        }

        if (introVideoPlayer != null && !introVideoPlayer.isPlaying)
        {
            introVideoPlayer.Play();
        }

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
     
        StartCoroutine(FadeToBlackAndShowScrollingText());
    }

    private IEnumerator FadeToBlackAndShowScrollingText()
    {
        Debug.Log("Starting Fade to Black");

      
        yield return StartCoroutine(FadeCanvasGroup(fadeCanvasGroup, 0f, 1f, fadeDuration));
        Debug.Log("Fade to Black Complete");

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


        Debug.Log("Fading to Transparent Before Scrolling Text");
        yield return StartCoroutine(FadeCanvasGroup(fadeCanvasGroup, 1f, 0f, fadeDuration));
        Debug.Log("Fade to Transparent Complete");


        scrollingText.gameObject.SetActive(true);
        yield return StartCoroutine(ScrollText());

        scrollingText.gameObject.SetActive(false);

   
        welcomeText.SetActive(true);

  
        yield return new WaitForSeconds(1f);

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
