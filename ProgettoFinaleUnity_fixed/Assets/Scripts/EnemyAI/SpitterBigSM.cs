using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterBigSM : SpitterSM
{
    public List<SpitterSmallSM> Followers;
    public List<Transform> FollowerDestinations;

    public List<Transform> patrolPoints;
    public float TimeBetweenPatrolPoints = 2f;
    public bool Loop;

    protected override BaseState GetInitialState()
    {
        return patrolState;
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        patrolState = new Spitter_PatrolBig(this);
    }
}
