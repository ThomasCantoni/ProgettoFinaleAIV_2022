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

    public static PlayerData LastSave;
    
    
    public static void SaveOptions(float sens,float fov)
    {
        PlayerPrefs.SetFloat(AimSensitivity, sens);
        PlayerPrefs.SetFloat(FOV, fov);
        PlayerPrefs.Save();
    }
    public static void SavePlayer(PlayerData toSave)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(Application.persistentDataPath + "/playerData.dat");
        bf.Serialize(fs, toSave);
        fs.Close();
    }
    public static PlayerData LoadPlayer(string path)
    {
        if(!File.Exists(path))
        {
            return null;
        }
        FileStream fs = File.Open(path,FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        LastSave = (PlayerData)bf.Deserialize(fs);
        fs.Close();
        return LastSave;
    }
}
