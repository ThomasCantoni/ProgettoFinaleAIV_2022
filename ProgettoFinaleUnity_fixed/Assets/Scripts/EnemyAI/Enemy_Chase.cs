using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Chase : BaseState
{
    NavMeshAgent agent;
    private float speed = 2f;
    private float timer = 0f;
    private float acceleration = 2f;
    private bool firstEnter = true;
    
    ChomperSM sm;
    public Enemy_Chase(ChomperSM stateMachine) : base("Enemy_Patrol", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        if (firstEnter)
        {
            firstEnter = false;
            timer = sm.AttackCooldown;
        }
        else
        {
            timer = 0f;
        }
        agent = sm.gameObject.GetComponent<NavMeshAgent>();
        agent.destination = sm.ObjToChase.position;
        sm.anim.SetBool("Run", true);
        agent.speed = speed * acceleration;
    }

    public override void UpdateLogic()
    {
        agent.destination = sm.ObjToChase.position;
        timer += Time.deltaTime;
        if (timer >= sm.AttackCooldown && Vector3.Distance(sm.transform.position, sm.ObjToChase.position) <= sm.AttackDistance)
        {
            sm.ChangeState(sm.attackState);
        }
    }

    public override void OnExit()
    {
        //METTERE IDLE
        sm.anim.SetBool("Idle", true);
        sm.anim.SetBool("Run", false);
        agent.destination = agent.transform.position;
        agent.speed = 0f;
    }
}
