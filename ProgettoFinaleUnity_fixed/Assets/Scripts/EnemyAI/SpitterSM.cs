using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterSM : EnemySM
{
    protected override void OnAwake()
    {
        base.OnAwake();
        chaseState = new Spitter_Chase(this);
        attackState = new Spitter_Attack(this);
        deathState = new Spitter_Death(this);
    }
}
