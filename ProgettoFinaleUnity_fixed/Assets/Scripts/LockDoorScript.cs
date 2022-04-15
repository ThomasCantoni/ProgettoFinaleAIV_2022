using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoorScript : MonoBehaviour
{
    public Animator toDisable;
    public void OnTriggerEnter(Collider other)
    {
        toDisable.enabled = false;
    }
}
