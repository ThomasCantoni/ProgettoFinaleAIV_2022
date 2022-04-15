using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<CameraShakeScript>().ApplyShake(2, 5, 5);
    }
}
