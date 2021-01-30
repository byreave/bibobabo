using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject Botton_Pause;
    public GameObject TakeAction;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC");
            if (GameIsPaused)
            {
               Resume();

            }else
            {
                Pause();
            }

        }
    }
   public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Botton_Pause.SetActive(true);
        TakeAction.SetActive(true);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Botton_Pause.SetActive(false);
        TakeAction.SetActive(false);
    }

    public void LoadMenu()
    {
        Debug.Log("Loading Menu");
    }
    public void ResetGame()
    {
        Debug.Log("ResetGame");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
}
