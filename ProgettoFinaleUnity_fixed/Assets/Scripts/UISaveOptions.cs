using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISaveOptions : MonoBehaviour
{
    public Slider AimSensiSlider, FOVSlider;
    public float AimSens { get; set; }
    public float Fov { get; set; }
    public void OnEnable()
    {
        AimSensiSlider.value = PlayerPrefs.GetFloat(SaveManager.AimSensitivity);
        FOVSlider.value = PlayerPrefs.GetFloat(SaveManager.FOV);
    }
    public void SaveOptions()
    {
        SaveManager.SaveOptions(AimSens, Fov);
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
