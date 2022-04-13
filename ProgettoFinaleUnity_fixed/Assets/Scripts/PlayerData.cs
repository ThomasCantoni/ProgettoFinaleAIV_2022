using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

[Serializable]
public class PlayerData
{
    public float playerPosX, playerPosY, playerPosZ;
    public float PlayerHp, PlayerAp;
    private bool newData = true;
    public int SceneIndex;
    public string SceneName;
    [SerializeField]
    public List<int> keysTaken = new List<int>();
    public bool IsNewGame
    {
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
        keysTaken = pcsv.PlayerData.keysTaken;
        SceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneName = SceneManager.GetActiveScene().name;
        newData = false;
    }
    public PlayerData()
    {


    }
}