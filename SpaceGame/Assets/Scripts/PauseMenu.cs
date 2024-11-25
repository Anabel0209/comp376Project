using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;         // Reference to the Pause Menu panel
    public Button pauseButton;           // Reference to the Pause button
    public Button resumeButton;          // Reference to the Resume button

    public Button helpButton;            // Reference to the Help button
    public GameObject helpPanel;         // Reference to the Help Panel
    public Button closeHelpButton;       // Reference to the Close Help button

    private cameraController cameraControllerScript; // Reference to the cameraController script

    private bool isPaused = false;

    private void Start()
    {
        // Find the cameraController script
        cameraControllerScript = FindObjectOfType<cameraController>();
        if (cameraControllerScript == null)
        {
            Debug.LogError("cameraController script is missing or not assigned!");
            return;
        }

        // Set up button listeners
        if (pauseButton != null) pauseButton.onClick.AddListener(TogglePause);
        if (resumeButton != null) resumeButton.onClick.AddListener(ResumeGame);

        if (helpButton != null) helpButton.onClick.AddListener(OpenHelpPanel);
        if (closeHelpButton != null) closeHelpButton.onClick.AddListener(CloseHelpPanel);


        // Ensure the pause menu is hidden initially
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (helpPanel != null) helpPanel.SetActive(false);

        // Update the pause button visibility at the start
        UpdatePauseButtonVisibility();
    }

    private void Update()
    {
        // Update the pause button visibility based on the current planet
        UpdatePauseButtonVisibility();
    }

    private void UpdatePauseButtonVisibility()
    {
        if (cameraControllerScript == null || pauseButton == null)
        {
            Debug.LogError("cameraController script or Pause button is not assigned!");
            return;
        }

        // Show the pause button only if the player is not on Planet 0
        pauseButton.gameObject.SetActive(cameraControllerScript.planetnb != 0);
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            // Show the pause menu and pause the game
            if (pauseMenu != null) pauseMenu.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            // Hide the pause menu and resume the game
            if (pauseMenu != null) pauseMenu.SetActive(false);
            Time.timeScale = 1f; // Resume the game
        }
    }

    private void ResumeGame()
    {
        isPaused = false;
        if (pauseMenu != null) pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    private void OpenHelpPanel()
    {
        // Show the help panel and hide the pause menu
        if (helpPanel != null) helpPanel.SetActive(true);
        if (pauseMenu != null) pauseMenu.SetActive(false);
    }

    private void CloseHelpPanel()
    {
        // Hide the help panel and return to the pause menu
        if (helpPanel != null) helpPanel.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(true);
    }
}
