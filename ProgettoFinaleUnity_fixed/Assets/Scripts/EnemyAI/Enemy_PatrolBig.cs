using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy_PatrolBig : Enemy_Patrol
{
    public Enemy_PatrolBig(string name, StateMachine stateMachine) : base(name, stateMachine)
    {
    }
}
