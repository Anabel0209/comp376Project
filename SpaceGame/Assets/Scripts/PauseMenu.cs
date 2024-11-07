using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;         // Reference to the Pause Menu panel
    public Button pauseButton;           // Reference to the Pause button
    public Button resumeButton;          // Reference to the Resume button
    public Button mainPlanetButton;      // Reference to the Go to Main Planet button
    public teleporter teleporterScript;  // Reference to the teleporter script to access teleport functions

    private bool isPaused = false;

    private void Start()
    {
        // Set up button listeners
        pauseButton.onClick.AddListener(TogglePause);
        resumeButton.onClick.AddListener(ResumeGame);
        mainPlanetButton.onClick.AddListener(GoToMainPlanet);

        // Ensure the pause menu is hidden initially
        pauseMenu.SetActive(false);

        // Check the planet at the start
        UpdatePauseButtonVisibility();
    }


    private void Update()
    {
        // Update the pause button visibility based on the current planet
        UpdatePauseButtonVisibility();
    }


    private void UpdatePauseButtonVisibility()
    {
        // Show the pause button only if the player is on Planet1
        pauseButton.gameObject.SetActive(teleporterScript.CurrentPlanet == 1);
    }



    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            // Show the pause menu and pause the game
            pauseMenu.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            // Hide the pause menu and resume the game
            pauseMenu.SetActive(false);
            Time.timeScale = 1f; // Resume the game
        }
    }

    private void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    private void GoToMainPlanet()
    {
        // Teleport the player to the main planet and reset the Z position to 0
        Vector3 mainPlanetPosition = teleporterScript.mainPlanet.position;
        teleporterScript.myCharacter.transform.position = new Vector3(mainPlanetPosition.x, mainPlanetPosition.y, 0);

        // Set the camera to focus on the main planet
        teleporterScript.SetCameraPlanet(0);

        // Update the current planet to 0 (main planet)
        teleporterScript.SetCurrentPlanet(0);

        // Update the pause button visibility to hide it on the main planet
        UpdatePauseButtonVisibility();

        // Resume the game
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

}
