using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IHittable
{
    public float LifeTime = 4f;
    public float Damage = 25f;
    public float Velocity = 5f;
    public GameObject ExplosionEffect;

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
        GameObject explosion = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        Destroy(explosion, 2f);
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

    public HittableType OnHit(Collider sender)
    {
        OnEndLife();
        return HittableType.Other;
    }
}
