using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Chase : BaseState
{
    NavMeshAgent agent;
    float timer = 0;

    ChomperSM sm;
    public Enemy_Chase(ChomperSM stateMachine) : base("Enemy_Patrol", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        agent = sm.gameObject.GetComponent<NavMeshAgent>();
        agent.destination = sm.ObjToChase.position;
        timer = 0;

        sm.gameObject.GetComponentInChildren<MeshRenderer>().material = sm.Debug_Materials[1];
    }

    public override void UpdateLogic()
    {
        agent.destination = sm.ObjToChase.position;
        timer += Time.deltaTime;
        if (timer >= 3f)
            sm.ChangeState(sm.patrolState);
    }

    public override void OnExit()
    {
        sm.ObjToChase = null;
    }
}
