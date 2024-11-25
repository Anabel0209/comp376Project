using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;        
    public Button pauseButton;          
    public Button resumeButton;         

    public Button helpButton;           
    public GameObject helpPanel;         
    public Button closeHelpButton;      

    private cameraController cameraControllerScript; 

    private bool isPaused = false;

    private void Start()
    {
        cameraControllerScript = FindObjectOfType<cameraController>();
        if (cameraControllerScript == null)
        {
            Debug.LogError("cameraController script is missing or not assigned!");
            return;
        }

     
        if (pauseButton != null) pauseButton.onClick.AddListener(TogglePause);
        if (resumeButton != null) resumeButton.onClick.AddListener(ResumeGame);

        if (helpButton != null) helpButton.onClick.AddListener(OpenHelpPanel);
        if (closeHelpButton != null) closeHelpButton.onClick.AddListener(CloseHelpPanel);


        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (helpPanel != null) helpPanel.SetActive(false);

        UpdatePauseButtonVisibility();
    }

    private void Update()
    {
        UpdatePauseButtonVisibility();
    }

    private void UpdatePauseButtonVisibility()
    {
        if (cameraControllerScript == null || pauseButton == null)
        {
            Debug.LogError("cameraController script or Pause button is not assigned!");
            return;
        }

        pauseButton.gameObject.SetActive(cameraControllerScript.planetnb != 0);
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
           
            if (pauseMenu != null) pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            if (pauseMenu != null) pauseMenu.SetActive(false);
            Time.timeScale = 1f; 
        }
    }

    private void ResumeGame()
    {
        isPaused = false;
        if (pauseMenu != null) pauseMenu.SetActive(false);
        Time.timeScale = 1f; 
    }

    private void OpenHelpPanel()
    {
        if (helpPanel != null) helpPanel.SetActive(true);
        if (pauseMenu != null) pauseMenu.SetActive(false);
    }

    private void CloseHelpPanel()
    {
        if (helpPanel != null) helpPanel.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(true);
    }
}
