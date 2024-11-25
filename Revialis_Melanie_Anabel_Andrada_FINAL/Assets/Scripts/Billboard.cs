using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public GameObject EndGameMenu;
    public GameObject Player; 

    private bool isGamePaused = false; 

    private void Start()
    {
        if (EndGameMenu != null)
        {
            EndGameMenu.SetActive(false); 
        }
    }

    public void OnMouseDown()
    {
        ToggleEndGameMenu(); 
    }

    public void ToggleEndGameMenu()
    {
        if (EndGameMenu != null)
        {
            isGamePaused = !isGamePaused;
            EndGameMenu.SetActive(isGamePaused);

           
            Time.timeScale = isGamePaused ? 0f : 1f;

            
            if (Player != null)
            {
                Player.GetComponent<PlayerMovement>().enabled = !isGamePaused;
            }
        }
    }

   
    public void ResumeGame()
    {
        if (EndGameMenu != null)
        {
            EndGameMenu.SetActive(false);
        }
        Time.timeScale = 1f;
        isGamePaused = false;

        
        if (Player != null)
        {
            Player.GetComponent<PlayerMovement>().enabled = true;
        }
    }

   
    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit(); 

#if UNITY_EDITOR
        // Stop play mode in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
