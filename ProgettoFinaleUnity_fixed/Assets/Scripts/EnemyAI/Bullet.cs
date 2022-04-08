using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float LifeTime = 4f;
    public float Velocity = 5f;

    void OnEnable()
    {
        Invoke("OnEndLife", LifeTime);
    }

    void Update()
    {
        transform.Translate(0, 0, Velocity * Time.deltaTime);
    }

    void OnEndLife()
    {
        gameObject.SetActive(false);
    }
}
