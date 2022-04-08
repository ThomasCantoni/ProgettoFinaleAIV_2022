using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy_Attack : BaseState
{
    public Enemy_Attack(string name, StateMachine stateMachine) : base(name, stateMachine)
    {
    }

    protected virtual void Attack()
    {
    }
}
