using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chomper_Chase : Enemy_Chase
{
    private float speed = 2f;
    private float acceleration = 2f;
    private float timer = 0f;
    private float chaseTimer = 0f;
    private bool firstEnter = true;

    ChomperSM sm;
    public Chomper_Chase(ChomperSM stateMachine) : base("Chomper_Chase", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        sm.ChangeStateText("CHASE");

        if (firstEnter)
        {
            firstEnter = false;
            timer = sm.AttackCooldown;
        }
        else
        {
            timer = 0f;
        }
        chaseTimer = 0f;
        sm.agent.destination = sm.ObjToChase.position;
        sm.anim.SetBool("Run", true);
        sm.agent.speed = speed * acceleration;
    }

    public override void UpdateLogic()
    {
        timer += Time.deltaTime;
        chaseTimer += Time.deltaTime;
        if (timer >= sm.AttackCooldown && Vector3.Distance(sm.transform.position, sm.ObjToChase.position) <= sm.AttackDistance)
        {
            sm.ChangeState(sm.attackState);
        }
        if (chaseTimer >= 5f && AttemptReturnPatrol())
        {
            sm.ChangeState(sm.patrolState);
        }
        //if (Vector3.Distance(sm.transform.position, sm.ObjToChase.position) >= sm.AttackDistance * 2 && chaseTimer >= 1f && AttemptReturnPatrol())
        //{
        //    sm.ChangeState(sm.patrolState);
        //}
        sm.agent.destination = sm.ObjToChase.position;
    }

    public override void OnExit()
    {
        sm.anim.SetBool("Run", false);
        sm.agent.destination = sm.transform.position;
        sm.agent.speed = 0f;
    }

    public virtual bool AttemptReturnPatrol()
    {
        if (sm is ChomperSmallSM)
        {
            if (((ChomperSmallSM)sm).Leader.gameObject.activeSelf)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }
}
