using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerBigSM : GunnerSM
{
    public List<GunnerSmallSM> Followers;
    public List<Transform> FollowerDestinations;

    public List<Transform> patrolPoints;
    public float TimeBetweenPatrolPoints = 2f;
    public bool Loop;

    protected override BaseState GetInitialState()
    {
        return patrolState;
    }

    public override void OnAwake()
    {
        base.OnAwake();
        patrolState = new Gunner_PatrolBig(this);
    }
}
