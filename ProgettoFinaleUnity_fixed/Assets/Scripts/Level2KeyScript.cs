using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2KeyScript : MonoBehaviour
{
    public int ID_KeyToRemove;
    private void OnTriggerEnter(Collider other)
    {
        PlayerControllerSecondVersion PCSV = other.GetComponent<PlayerControllerSecondVersion>();
        if (PCSV.PlayerData.keysTaken.Contains(ID_KeyToRemove))
        {
            PCSV.RemoveKey(ID_KeyToRemove);
        }
        else
        {
            Debug.Log("Key not found, nothing to remove");
        }
    }
}
