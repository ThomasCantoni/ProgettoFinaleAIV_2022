using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spitter_PatrolSmall : Enemy_PatrolSmall
{
    private float speed = 2f;

    protected SpitterSmallSM sm;
    public Spitter_PatrolSmall(SpitterSmallSM stateMachine) : base("Spitter_PatrolSmall", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        sm.ChangeStateText("PATROL");

        sm.DetectCollider.enabled = true;
        sm.OnShpereTriggerStay += OnDetection;
        sm.AttackCollider.enabled = false;
        sm.agent.speed = speed;
        sm.agent.destination = sm.Leader.FollowerDestinations[sm.ID].position;
    }

    public override void UpdateLogic()
    {
        sm.agent.destination = sm.Leader.FollowerDestinations[sm.ID].position;
    }

    public override void OnExit()
    {
        sm.OnShpereTriggerStay -= OnDetection;
    }

    public override void OnDetection(GameObject sender, Collider c, string message, bool fromEvent)
    {
        sm.ObjToChase = c.gameObject.transform;
        if (fromEvent)
        {
            if (sm.Leader.gameObject.activeSelf)
                sm.Leader.patrolState.OnDetection(sender, c, message, false);
            for (int i = 0; i < sm.Leader.Followers.Count; i++)
            {
                if (i == sm.ID)
                    continue;
                if (sm.Leader.Followers[i].gameObject.activeSelf)
                    sm.Leader.Followers[i].patrolState.OnDetection(sender, c, message, false);
            }
        }
        sm.ChangeState(sm.chaseState);
    }
}
