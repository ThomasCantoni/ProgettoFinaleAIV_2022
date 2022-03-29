using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chomper : MonoBehaviour, IHittable
{
    public int Health;
    public UnityEvent<Collider> OnHitEvent;

    public virtual void OnHit(Collider sender)
    {
        OnHitEvent?.Invoke(sender);

        Health--;
        Debug.Log(Health);

        if (Health <= 0)
        {
            //Destroy(transform.parent.gameObject);
            gameObject.SetActive(false);
        }
    }
}
