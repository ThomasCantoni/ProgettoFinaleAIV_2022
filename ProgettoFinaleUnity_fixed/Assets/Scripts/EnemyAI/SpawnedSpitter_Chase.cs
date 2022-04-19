using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnedSpitter_Chase : Enemy_Chase
{
    private float speed = 2f;
    private float acceleration = 2f;
    private float timer = 0f;
    private bool firstEnter = true;

    SpawnedSpitterSM sm;
    public SpawnedSpitter_Chase(SpawnedSpitterSM stateMachine) : base("SpawnedSpitter_Chase", stateMachine)
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
        sm.anim.SetBool("Run", true);
        sm.agent.speed = speed * acceleration;
    }

    public override void UpdateLogic()
    {
        timer += Time.deltaTime;
        if (timer >= sm.AttackCooldown && Vector3.Distance(sm.transform.position, sm.ObjToChase.position) <= sm.AttackDistance)
        {
            sm.ChangeState(sm.attackState);
        }
        sm.agent.destination = sm.ObjToChase.position;
    }

    public override void OnExit()
    {
        sm.anim.SetBool("Run", false);
        sm.agent.destination = sm.transform.position;
        sm.agent.speed = 0f;
    }
}
