using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChomperSM : StateMachine
{
    public delegate void OnSphereTriggerDelecate(GameObject sender, Collider collider, string message, bool fromEvent);
    public event OnSphereTriggerDelecate OnSphereTriggerEnter;

    [HideInInspector]
    public Enemy_Patrol patrolState;
    [HideInInspector]
    public Enemy_Chase chaseState;
    [HideInInspector]
    public Transform ObjToChase;

    public Material[] Debug_Materials;
    public Animator anim;

    void Awake()
    {
        OnAwake();
        anim = anim.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider c)
    {
        OnSphereTriggerEnter?.Invoke(this.gameObject, c, "Collision with: " + c.name, true);
    }

    protected virtual void OnAwake()
    {
        chaseState = new Enemy_Chase(this);
    }
}
