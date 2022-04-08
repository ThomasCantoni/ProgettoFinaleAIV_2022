using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy_Patrol : BaseState
{
    public Enemy_Patrol(string name, StateMachine stateMachine) : base(name, stateMachine)
    {
    }

    public virtual void OnDetection(GameObject sender, Collider c, string message, bool fromEvent)
    {
    }
}
