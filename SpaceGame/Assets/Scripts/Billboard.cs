using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public GameObject EndGameMenu;
    public GameObject Player; // Reference to the player GameObject to disable movement (if applicable)

    private bool isGamePaused = false; // Track the game state

    private void Start()
    {
        if (EndGameMenu != null)
        {
            EndGameMenu.SetActive(false); // Ensure the menu is inactive at the start
        }
    }

    public void OnMouseDown()
    {
        ToggleEndGameMenu(); // Show the menu when the billboard is clicked
    }

    public void ToggleEndGameMenu()
    {
        if (EndGameMenu != null)
        {
            isGamePaused = !isGamePaused;
            EndGameMenu.SetActive(isGamePaused);

            // Pause or resume the game
            Time.timeScale = isGamePaused ? 0f : 1f;

            // Disable or enable player movement
            if (Player != null)
            {
                Player.GetComponent<PlayerMovement>().enabled = !isGamePaused;
            }
        }
    }

    // Resume button functionality
    public void ResumeGame()
    {
        if (EndGameMenu != null)
        {
            EndGameMenu.SetActive(false);
        }
        Time.timeScale = 1f; // Resume the game
        isGamePaused = false;

        // Enable player movement
        if (Player != null)
        {
            Player.GetComponent<PlayerMovement>().enabled = true;
        }
    }

    // Quit button functionality
    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit(); // Quit the application

#if UNITY_EDITOR
        // Stop play mode in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
