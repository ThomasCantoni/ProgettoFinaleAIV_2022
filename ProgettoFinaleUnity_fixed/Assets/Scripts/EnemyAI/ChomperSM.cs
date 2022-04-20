using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class ChomperSM : EnemySM
{
    public override void OnAwake()
    {
        base.OnAwake();
        chaseState = new Chomper_Chase(this);
        attackState = new Chomper_Attack(this);
        deathState = new Chomper_Death(this);
    }
}
