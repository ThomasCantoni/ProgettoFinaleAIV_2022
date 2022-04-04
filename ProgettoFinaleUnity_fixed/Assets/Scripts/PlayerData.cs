using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class PlayerData 
{
    public float playerPosX, playerPosY, playerPosZ;
    public float PlayerHp, PlayerAp;
    private bool newData = true;
    public int SceneIndex;
    public bool IsNewGame {
        get
        {
            return newData;
        }
    }
    public PlayerData(PlayerControllerSecondVersion pcsv)
    {
        PlayerHp = pcsv.GetComponent<EllenHealthScript>().HP_Value;
        PlayerAp = pcsv.GetComponent<EllenActionPoints>().AP_Value;
        playerPosX = pcsv.transform.position.x;
        playerPosY = pcsv.transform.position.y;
        playerPosZ = pcsv.transform.position.z;
        newData = false;
    }
    public PlayerData()
    {
        

    }
}