using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Gunner_PatrolSmall : Enemy_PatrolSmall
{
    private float speed = 4f;
    private float acceleration = 2f;

    protected NavMeshAgent agent;
    protected GunnerSmallSM sm;
    public Gunner_PatrolSmall(GunnerSmallSM stateMachine) : base("Gunner_PatrolSmall", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        sm.DetectCollider.enabled = true;
        sm.OnShpereTriggerStay += OnDetection;
        sm.AttackCollider.enabled = false;
        agent = sm.gameObject.GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.destination = sm.Leader.FollowerDestinations[sm.ID].position;
    }

    public override void UpdateLogic()
    {
        agent.destination = sm.Leader.FollowerDestinations[sm.ID].position;
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
