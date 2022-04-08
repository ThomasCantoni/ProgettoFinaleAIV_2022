using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy_PatrolSmall : Enemy_Patrol
{
    public Enemy_PatrolSmall(string name, StateMachine stateMachine) : base(name, stateMachine)
    {
    }
}
