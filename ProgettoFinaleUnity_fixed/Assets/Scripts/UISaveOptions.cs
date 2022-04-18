using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UISaveOptions : MonoBehaviour
{
    UI_MasterVolumeScript vol;
    public Slider AimSensiSlider, FOVSlider,VolumeSlider;
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
    public int DropdownReso;
    public void OnEnable()
    {
        vol = GetComponent<UI_MasterVolumeScript>();
        AimSensiSlider.value = PlayerPrefs.GetFloat(SaveManager.AimSensitivity);
        FOVSlider.value = PlayerPrefs.GetFloat(SaveManager.FOV);
        VolumeSlider.value = PlayerPrefs.GetFloat(SaveManager.Volume);
    }
    public void SaveOptions()
    {
        SaveManager.SaveOptions(AimSens, Fov,Volume);

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
