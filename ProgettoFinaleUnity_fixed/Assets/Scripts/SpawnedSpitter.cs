using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedSpitter : Enemy
{
    public int MaxHealth;

    private void OnEnable()
    {
        Health = MaxHealth;
        OnStart();
    }
}
