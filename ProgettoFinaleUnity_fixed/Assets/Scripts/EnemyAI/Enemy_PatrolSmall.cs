using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_PatrolSmall : Enemy_Patrol
{
    ChomperSmallSM smSmall;
    public Enemy_PatrolSmall(ChomperSmallSM stateMachine) : base("Enemy_PatrolSmall", stateMachine)
    {
        smSmall = stateMachine;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        agent.destination = smSmall.Leader.FollowerDestinations[smSmall.ID].position;
    }

    public override void UpdateLogic()
    {
        agent.destination = smSmall.Leader.FollowerDestinations[smSmall.ID].position;
    }

    public override void OnDetection(GameObject sender, Collider c, string message, bool fromEvent)
    {
        base.OnDetection(sender, c, message, fromEvent);
        if (fromEvent)
        {
            smSmall.Leader.patrolState.OnDetection(sender, c, message, false);
            for (int i = 0; i < smSmall.Leader.Followers.Count; i++)
            {
                if (i == smSmall.ID)
                    continue;
                smSmall.Leader.Followers[i].patrolState.OnDetection(sender, c, message, false);
            }
        }
        sm.ChangeState(sm.chaseState);
    }
}
