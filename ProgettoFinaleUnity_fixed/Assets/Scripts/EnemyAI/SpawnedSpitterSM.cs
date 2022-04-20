using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class SpawnedSpitterSM : EnemySM
{
    public float IdleTime;
    [HideInInspector]
    public Enemy_Idle idleState;

    public GameObject SpawnSmokeEffect;

    protected override BaseState GetInitialState()
    {
        return idleState;
    }

    public override void OnAwake()
    {
        base.OnAwake();
        idleState = new SpawnedSpitter_Idle(this);
        chaseState = new SpawnedSpitter_Chase(this);
        attackState = new SpawnedSpitter_Attack(this);
        deathState = new SpawnedSpitter_Death(this);
    }

    public virtual void OnSpawn()
    {
        GameObject impactGO = Instantiate(SpawnSmokeEffect, transform.position, Quaternion.LookRotation(Vector3.up));
        Destroy(impactGO, 1f);
    }
}
