using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private void OnTriggerStay(Collider collision)
    {
        EllenHealthScript attempt = new EllenHealthScript();
        if (collision.gameObject.TryGetComponent(out attempt))
        {
            attempt.DamagePlayer(10f * Time.deltaTime);
        }
    }
}
