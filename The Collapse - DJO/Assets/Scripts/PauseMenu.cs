using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu; 
    public static bool isPaused; 

    void Start()
    {
        DontDestroyOnLoad(pauseMenu);
        Time.timeScale = 1f; // Jogo inicia sem pausa
        isPaused = false; 
        pauseMenu.SetActive(false); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame(); 
            }
            else
            {
                PauseGame(); 
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true); 
        Time.timeScale = 0f; 
        isPaused = true; 
        Cursor.visible = true; 
        Cursor.lockState = CursorLockMode.None; 
    }

    // Método chamado para retomar o jogo
    public void ResumeGame()
    {
        pauseMenu.SetActive(false); 
        Time.timeScale = 1f; 
        isPaused = false; 
        Cursor.visible = false; 
    }
}
