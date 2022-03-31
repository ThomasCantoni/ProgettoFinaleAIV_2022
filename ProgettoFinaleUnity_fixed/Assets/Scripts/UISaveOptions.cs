using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISaveOptions : MonoBehaviour
{
    public float AimSens { get; set; }
    public float Fov { get; set; }

    public void SaveOptions()
    {
        SaveManager.SaveOptions(AimSens, Fov);
    }
    public void Revert()
    {
        AimSens = PlayerPrefs.GetFloat(SaveManager.AimSensitivity);
        Fov = PlayerPrefs.GetFloat(SaveManager.FOV);

    }
}
