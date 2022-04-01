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
    public bool IsNewGame {
        get
        {
            return newData;
        }
    }
    public PlayerData(PlayerControllerSecondVersion pcsv)
    {
        PlayerHp = pcsv.GetComponent<EllenHealthScript>().HP_Value;
        //playerap to add
        playerPosX = pcsv.transform.position.x;
        playerPosY = pcsv.transform.position.y;
        playerPosZ = pcsv.transform.position.z;
        newData = false;
    }
    public PlayerData()
    {
        

    }
}