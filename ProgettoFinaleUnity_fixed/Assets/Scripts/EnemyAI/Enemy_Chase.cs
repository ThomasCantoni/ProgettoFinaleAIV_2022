using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Chase : BaseState
{
    NavMeshAgent agent;
    private float speed = 2f;
    private float acceleration = 2f;
    
    ChomperSM sm;
    public Enemy_Chase(ChomperSM stateMachine) : base("Enemy_Patrol", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        agent = sm.gameObject.GetComponent<NavMeshAgent>();
        agent.destination = sm.ObjToChase.position;
        sm.anim.SetBool("Run", true);
        //sm.anim.SetTrigger("Attack");
        agent.speed = speed * acceleration;
        sm.gameObject.GetComponentInChildren<MeshRenderer>().material = sm.Debug_Materials[1];
    }

    public override void UpdateLogic()
    {
        agent.destination = sm.ObjToChase.position;
        if (Vector3.Distance(sm.transform.position, sm.ObjToChase.position) <= sm.AttackDistance)
        {
            sm.ChangeState(sm.attackState);
        }

    }

    public override void OnExit()
    {
        //METTERE IDLE
        //sm.anim.SetBool("Idle", true);

        sm.anim.SetBool("Run", false);
        agent.speed = speed;
    }
}
