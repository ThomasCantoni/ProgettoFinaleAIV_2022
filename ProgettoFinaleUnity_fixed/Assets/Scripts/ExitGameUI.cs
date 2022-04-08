using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class ExitGameUI : MonoBehaviour
{
    public void CloseGame()
    {
        Application.Quit();
    }
    public void BackToMenu()
    {
        TimeManager.DisablePause();
        SceneManager.LoadScene("Scenes/MainMenuWIP", LoadSceneMode.Single);
    }
}
