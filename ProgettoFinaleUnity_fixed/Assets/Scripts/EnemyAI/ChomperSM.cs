using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperSM : StateMachine
{
    public delegate void OnSphereTriggerDelecate(GameObject sender, Collider collider, string message);
    public event OnSphereTriggerDelecate OnSphereTriggerEnter;

    [HideInInspector]
    public Enemy_Patrol patrolState;
    [HideInInspector]
    public Enemy_Chase chaseState;
    [HideInInspector]
    public Transform ObjToChase;

    public Material[] Debug_Materials;

    public List<Transform> patrolPoints;
    public float TimeBetweenPatrolPoints = 2f;
    public bool Loop;

    void Awake()
    {
        patrolState = new Enemy_Patrol(this);
        chaseState = new Enemy_Chase(this);
    }

    protected override BaseState GetInitialState()
    {
        return patrolState;
    }

    void OnTriggerEnter(Collider c)
    {
        OnSphereTriggerEnter?.Invoke(this.gameObject, c, "Collision with: " + c.name);
    }
}
