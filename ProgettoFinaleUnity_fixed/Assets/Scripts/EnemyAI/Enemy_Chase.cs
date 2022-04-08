using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy_Chase : BaseState
{
    public Enemy_Chase(string name, StateMachine stateMachine) : base(name, stateMachine)
    {
    }
}
