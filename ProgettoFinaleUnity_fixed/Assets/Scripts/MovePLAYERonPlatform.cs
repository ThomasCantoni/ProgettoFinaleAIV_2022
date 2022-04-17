using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePLAYERonPlatform : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        other.gameObject.transform.parent = this.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;

    }
}
