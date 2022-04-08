using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireFly : MonoBehaviour, IHittable
{
    public GameObject fireFly;
    public int Health;
    public UnityEvent<Collider> OnHitEvent;
    public virtual void OnHit(Collider sender)
    {
        OnHitEvent?.Invoke(sender);
        Health--;

        if (Health <= 0)
        {
            if (fireFly != null)
            {
                fireFly.SetActive(true);
            }
            this.gameObject.SetActive(false);
        }
    }
}
