using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterSmallSM : SpitterSM
{
    public SpitterBigSM Leader;
    public int ID;

    protected override BaseState GetInitialState()
    {
        return patrolState;
    }

    public override void OnAwake()
    {
        base.OnAwake();
        patrolState = new Spitter_PatrolSmall(this);
    }
}
