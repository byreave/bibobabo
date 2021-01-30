using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayLevel_1()
    {
        SceneManager.LoadScene("UITEST");
        Debug.Log("Entering Level 1");
    }
    public void PlayLevel_2()
    {
        //SceneManager.LoadScene("Level_2");
        Debug.Log("Entering Level 2");
    }
    public void PlayLevel_3()
    {
        //SceneManager.LoadScene("Level_3");
        Debug.Log("Entering Level 3");
    }


    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
    
}
