using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperSmallSM : ChomperSM
{
    public ChomperBigSM Leader;
    public int ID;

    protected override BaseState GetInitialState()
    {
        return patrolState;
    }

    public override void OnAwake()
    {
        base.OnAwake();
        patrolState = new Chomper_PatrolSmall(this);
    }
}
