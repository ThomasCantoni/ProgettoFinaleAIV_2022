using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerSmallSM : GunnerSM
{
    public GunnerBigSM Leader;
    public int ID;

    protected override BaseState GetInitialState()
    {
        return patrolState;
    }

    public override void OnAwake()
    {
        base.OnAwake();
        patrolState = new Gunner_PatrolSmall(this);
    }
}
