using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainCanvas, OptionsCanvas;

    public void StartNewGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    public void LoadGame()
    {

    }
    public void EnableOptions()
    {
        OptionsCanvas.SetActive(true);
        MainCanvas.SetActive(false);
    }
    public void EnableMainCanvas()
    {
        OptionsCanvas.SetActive(false);
        MainCanvas.SetActive(true);

    }
}
