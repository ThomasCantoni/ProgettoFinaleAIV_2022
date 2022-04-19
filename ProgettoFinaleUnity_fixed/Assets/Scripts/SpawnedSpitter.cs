using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedSpitter : Enemy
{
    public int MaxHealth;
    public GameObject BiteEffect;
    public Transform TonguePosition;

    public void OnBite()
    {
        GameObject biteSpit = Instantiate(BiteEffect, TonguePosition);
        Destroy(biteSpit, 0.5f);
    }

    private void OnEnable()
    {
        Health = MaxHealth;
        OnStart();
    }
}
