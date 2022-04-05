using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        PlayerData newData = new PlayerData(other.GetComponent<PlayerControllerSecondVersion>());
        SaveManager.SavePlayer(newData);
        other.GetComponent<PlayerControllerSecondVersion>().PlayerData = SaveManager.LastSave; 
    }
}
