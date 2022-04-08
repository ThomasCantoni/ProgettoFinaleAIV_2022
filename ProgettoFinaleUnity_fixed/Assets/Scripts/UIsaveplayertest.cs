using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIsaveplayertest : MonoBehaviour
{
    public PlayerControllerSecondVersion PCSV;
    PlayerData toSave;
    public void SavePlayerData()
    {

        toSave = new PlayerData(PCSV);
        SaveManager.SavePlayer(toSave);
        PCSV.PlayerData = toSave;
    }
}
