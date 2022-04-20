using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerSimpleScript : MonoBehaviour
{
    public UnityEvent Events;

    public void OnTriggerEnter(Collider other)
    {
        Events?.Invoke();
    }
}
