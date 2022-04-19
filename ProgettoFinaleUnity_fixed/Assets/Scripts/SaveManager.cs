using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveManager
{
    public static string AimSensitivity = "AimSens";
    public static string FOV = "FOV";
    public static string Volume = "Volume";


    public static PlayerData LastSave;


    public static void SaveOptions(float sens, float fov,float volume,ResItem res,int indexResItem,int Vsync)
    {
        PlayerPrefs.SetFloat(AimSensitivity, sens);
        PlayerPrefs.SetFloat(FOV, fov);
        PlayerPrefs.SetFloat(Volume, volume);
        PlayerPrefs.SetInt("ResWidth", res.width);
        PlayerPrefs.SetInt("ResHeight", res.height);
        PlayerPrefs.SetInt("Resolution", indexResItem);
        PlayerPrefs.SetInt("VsyncCount", Vsync);
        PlayerPrefs.Save();
    }
    public static void SaveOptions(UISaveOptions s)
    {
        PlayerPrefs.SetFloat(AimSensitivity, s.AimSens);
        PlayerPrefs.SetFloat(FOV, s.Fov);
        PlayerPrefs.SetFloat(Volume, s.Volume);
        if(s.Resolutions.Count>0)
        {
            PlayerPrefs.SetInt("ResWidth", s.Resolutions[s.SelectedResolutionDropdown].width);
            PlayerPrefs.SetInt("ResHeight", s.Resolutions[s.SelectedResolutionDropdown].height);
            PlayerPrefs.SetInt("Resolution", s.SelectedResolutionDropdown);
        }
        if (s.VsyncToggle != null)
        {
            PlayerPrefs.SetInt("VsyncCount", s.Vsync);
        }
        PlayerPrefs.Save();
    }
    public static void SavePlayer(PlayerData toSave)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(Application.persistentDataPath + "/playerData.dat");
        bf.Serialize(fs, toSave);
        LastSave = toSave;

        fs.Close();
    }
    public static PlayerData LoadPlayer(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }
        FileStream fs = File.Open(path, FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        LastSave = (PlayerData)bf.Deserialize(fs);
        fs.Close();
        return LastSave;
    }
}
