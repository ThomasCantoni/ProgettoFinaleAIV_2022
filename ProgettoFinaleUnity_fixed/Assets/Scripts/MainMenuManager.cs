using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using System;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainCanvas, OptionsCanvas, LoadingImage;
    public Button ContinueButton;
    public AudioMixer DefaultMixer, UI_Mixer;
    public GameObject MainMenuFirstSelected, OptionsMenuFirstSelected, MainMenuOptionsClosedSelected;

    private void Start()
    {
        SetVolume();
        Cursor.visible = true;
        if(SaveManager.LoadPlayer(Application.persistentDataPath + "/playerData.dat") == null)
        {
            ContinueButton.interactable = false;
        }
        else
        {
            ContinueButton.interactable = true;
        }
    }

    private void SetVolume()
    {
        float v = PlayerPrefs.GetFloat(SaveManager.Volume);
        DefaultMixer.SetFloat("Volume", v);
        UI_Mixer.SetFloat("UI Volume", v);
    }

    public void StartNewGame()
    {
        SaveManager.LastSave = new PlayerData();
        LevelManager.Instance.LoadScene("Scenes/ThomasCantoniMAP");
        MainCanvas.SetActive(false);
        OptionsCanvas.SetActive(false);
        LoadingImage.SetActive(true);



        //SceneManager.LoadScene("Scenes/Vertical Slice", LoadSceneMode.Single);


    }
    public void LoadGame()
    {
        SaveManager.LoadPlayer(Application.persistentDataPath + "/playerData.dat");
        string sceneName = SaveManager.LastSave.SceneName;
        LevelManager.Instance.LoadScene("Scenes/"+sceneName);
        MainCanvas.SetActive(false);
        OptionsCanvas.SetActive(false);
        LoadingImage.SetActive(true);



    }
    public void EnableOptions()
    {
        OptionsCanvas.SetActive(true);
        MainCanvas.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(OptionsMenuFirstSelected);
    }
    public void EnableMainCanvas()
    {
        OptionsCanvas.SetActive(false);
        MainCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(MainMenuOptionsClosedSelected);
    }
}
