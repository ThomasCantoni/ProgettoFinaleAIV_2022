using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisposableSaveTriggerScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerControllerSecondVersion PCSV = other.GetComponent<PlayerControllerSecondVersion>();
        PlayerData pd = new PlayerData(PCSV);
        SaveManager.SavePlayer(pd);
        Destroy(this.gameObject);
    }
}
