using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerSimpleScript : MonoBehaviour
{
    public UnityEvent Events;

    public void OnTriggerExit(Collider other)
    {
        Events?.Invoke();
    }
}
