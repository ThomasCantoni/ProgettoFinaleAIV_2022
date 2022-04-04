using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainCanvas, OptionsCanvas,LoadingImage;

    public void StartNewGame()
    {
        SaveManager.LastSave = new PlayerData();
        LevelManager.Instance.LoadScene("Scenes/Vertical Slice");
        MainCanvas.SetActive(false);
        OptionsCanvas.SetActive(false);
        LoadingImage.SetActive(true);
       
       

        //SceneManager.LoadScene("Scenes/Vertical Slice", LoadSceneMode.Single);
        

    }
    public void LoadGame()
    {
        SaveManager.LoadPlayer(Application.persistentDataPath + "/playerData.dat");
        LevelManager.Instance.LoadScene("Scenes/Vertical Slice");
        MainCanvas.SetActive(false);
        OptionsCanvas.SetActive(false);
        LoadingImage.SetActive(true);
       


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
