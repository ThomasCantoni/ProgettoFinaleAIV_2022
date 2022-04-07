using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    public Transform TeleportDestination;


    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("TELEPORTING " + other.gameObject.name);
        other.gameObject.GetComponent<CharacterController>().enabled = false;
        other.gameObject.transform.position = TeleportDestination.position;
        other.gameObject.GetComponent<CharacterController>().enabled = true;

    }
}
