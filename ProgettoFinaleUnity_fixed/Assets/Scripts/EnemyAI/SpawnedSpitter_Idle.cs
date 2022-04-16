using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnedSpitter_Idle : Enemy_Idle
{
    private float timer = 0f;

    SpawnedSpitterSM sm;
    public SpawnedSpitter_Idle(SpawnedSpitterSM stateMachine) : base("SpawnedSpitter_Idle", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        sm.anim.SetTrigger("SuperIdle");
        sm.anim.SetBool("Idle", false);
        sm.BodyCollider.enabled = true;
        sm.agent.speed = 0;
        timer = 0f;

        sm.WriteStateOnCanvas("Idle");
    }

    public override void UpdateLogic()
    {
        timer += Time.deltaTime;
        if (timer >= sm.IdleTime)
        {
            sm.ChangeState(sm.chaseState);
        }
    }

    public override void OnExit()
    {
    }
}
