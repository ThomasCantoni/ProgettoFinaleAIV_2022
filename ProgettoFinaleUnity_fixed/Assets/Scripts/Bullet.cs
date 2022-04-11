using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float LifeTime = 4f;
    public float Damage = 25f;
    public float Velocity = 5f;

    void OnEnable()
    {
        LifeTime = 4f;
    }

    void Update()
    {
        transform.Translate(0, 0, Velocity * Time.deltaTime);
        LifeTime -= Time.deltaTime;
        if (LifeTime < 0)
        {
            OnEndLife();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        EllenHealthScript hs = other.GetComponent<EllenHealthScript>();
        if (hs != null)
        {
            hs.DamagePlayer(25f);
        }
        OnEndLife();
    }

    void OnEndLife()
    {
        gameObject.SetActive(false);
    }
}
