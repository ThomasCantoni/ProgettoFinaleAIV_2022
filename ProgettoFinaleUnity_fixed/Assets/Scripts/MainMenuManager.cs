using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainCanvas, OptionsCanvas;

    public void StartNewGame()
    {
        SaveManager.LastSave = new PlayerData();
        SceneManager.LoadScene("Scenes/Vertical Slice", LoadSceneMode.Single);

    }
    public void LoadGame()
    {
        SaveManager.LoadPlayer(Application.persistentDataPath + "/playerData.dat");
        SceneManager.LoadScene("Scenes/Vertical Slice", LoadSceneMode.Single);

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
