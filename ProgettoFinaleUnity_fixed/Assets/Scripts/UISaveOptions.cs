using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ResItem
{
    public int width, height;
}
public class UISaveOptions : MonoBehaviour
{
    UI_MasterVolumeScript vol;
    public Slider AimSensiSlider, FOVSlider,VolumeSlider;
    public TMP_Dropdown Dropdown;
   
    public Toggle VsyncToggle;
    public int SelectedResolutionDropdown
    {
        get { return Dropdown.value; }
        set { Dropdown.value = value; }
    }
    public float AimSens { get; set; }
    public float Fov { get; set; }
    public float Volume
    {
        get
        {
            return VolumeSlider.value;
        }
        set
        {
            vol.ChangeVolume(value);
        }
    }
    public int Vsync
    { 
        get
        {
            if (VsyncToggle.isOn)
                return 1;
            else
                return 0;
        }
        set
        {
            if (value > 0)
                VsyncToggle.isOn = true;
            else
                VsyncToggle.isOn = false;

        }
    }
    [SerializeField]
    public List<ResItem> Resolutions = new List<ResItem>();

    public void OnEnable()
    {
        vol = GetComponent<UI_MasterVolumeScript>();
        if (Dropdown != null)
        {
            Dropdown.ClearOptions();
            for (int i = 0; i < Resolutions.Count; i++)
            {
                Dropdown.options.Add(new TMP_Dropdown.OptionData(Resolutions[i].width.ToString() + "x" + Resolutions[i].height.ToString()));
                // Dropdown.options[i].text = Resolutions[i].width.ToString() + "x" + Resolutions[i].height.ToString();
            }
            SelectedResolutionDropdown = PlayerPrefs.GetInt("Resolution");
        }
        AimSensiSlider.value = PlayerPrefs.GetFloat(SaveManager.AimSensitivity);
        FOVSlider.value = PlayerPrefs.GetFloat(SaveManager.FOV);
        VolumeSlider.value = PlayerPrefs.GetFloat(SaveManager.Volume);
        if(VsyncToggle != null)
        {
         Vsync = PlayerPrefs.GetInt("VsyncCount");
        }
        

    }
    public void SaveOptions()
    {
        SaveManager.SaveOptions(this);
        if(Resolutions.Count >0)
        { 
            ApplyGraphics();
        }
    }
    void ApplyGraphics()
    {
        Screen.fullScreen = true;
        QualitySettings.vSyncCount = Vsync;
        Screen.SetResolution(Resolutions[SelectedResolutionDropdown].width, Resolutions[SelectedResolutionDropdown].height, true);
    }
    public void Back()
    {
        OnEnable();

    }
    public void OnDisable()
    {
        OnEnable();
    }
}
