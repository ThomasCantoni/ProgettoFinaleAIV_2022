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
        OnStartLife();
    }

    void Update()
    {
        UpdateLogic();
    }

    void OnTriggerEnter(Collider other)
    {
        OnCollision(other);
    }

    protected virtual void OnStartLife()
    {
        LifeTime = 4f;
    }

    protected virtual void OnEndLife()
    {
        gameObject.SetActive(false);
    }

    protected virtual void UpdateLogic()
    {
        transform.Translate(0, 0, Velocity * Time.deltaTime);
        LifeTime -= Time.deltaTime;
        if (LifeTime < 0)
        {
            OnEndLife();
        }
    }

    protected virtual void OnCollision(Collider other)
    {
        EllenHealthScript hs = other.GetComponent<EllenHealthScript>();
        if (hs != null)
        {
            hs.DamagePlayer(Damage);
        }
        OnEndLife();
    }
}
