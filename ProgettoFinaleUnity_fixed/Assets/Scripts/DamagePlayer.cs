using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamagePlayer : MonoBehaviour
{
    private void OnTriggerStay(Collider collision)
    {
        collision.GetComponent<EllenHealthScript>().DamagePlayer(25f * Time.deltaTime);
    }
}
