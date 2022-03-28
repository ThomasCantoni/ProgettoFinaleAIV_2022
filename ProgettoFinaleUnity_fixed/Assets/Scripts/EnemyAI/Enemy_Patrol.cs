using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy_Patrol : BaseState
{
    protected NavMeshAgent agent;
    

    protected ChomperSM sm;
    public Enemy_Patrol(string name, ChomperSM stateMachine) : base(name, stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        sm.OnSphereTriggerEnter += OnDetection;
        
        agent = sm.gameObject.GetComponent<NavMeshAgent>();
        sm.gameObject.GetComponentInChildren<MeshRenderer>().material = sm.Debug_Materials[0];
    }

    public override void OnExit()
    {
        sm.OnSphereTriggerEnter -= OnDetection;
        
    }

    public virtual void OnDetection(GameObject sender, Collider c, string message, bool fromEvent)
    {
        sm.ObjToChase = c.gameObject.transform;
    }
}
