using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamagePlayer : MonoBehaviour
{
    public float Damage = 25f;
    private void OnTriggerStay(Collider collision)
    {
        collision.GetComponent<EllenHealthScript>().DamagePlayer(Damage * Time.deltaTime);
    }
}
