using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Gunner_Chase : Enemy_Chase
{
    NavMeshAgent agent;
    private float speed = 4f;
    private float acceleration = 1.2f;
    private float timer = 0f;

    GunnerSM sm;
    public Gunner_Chase(GunnerSM stateMachine) : base("Gunner_Chase", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        timer = 0f;
        agent = sm.gameObject.GetComponent<NavMeshAgent>();
        agent.destination = sm.ObjToChase.position;
        sm.anim.SetBool("WalkFast", true);
        agent.speed = speed * acceleration;
    }

    public override void UpdateLogic()
    {
        timer += Time.deltaTime;
        if (Vector3.Distance(sm.transform.position, sm.ObjToChase.position) <= sm.AttackDistance)
        {
            sm.ChangeState(sm.attackState);
        }
        if (timer >= 5f)
        {
            sm.ChangeState(sm.patrolState);
        }
        if (Vector3.Distance(sm.transform.position, sm.ObjToChase.position) >= sm.AttackDistance * 2)
        {
            sm.ChangeState(sm.patrolState);
        }
        agent.destination = sm.ObjToChase.position;
    }

    public override void OnExit()
    {
        //METTERE IDLE
        //sm.anim.SetBool("Idle", true);
        sm.anim.SetBool("WalkFast", false);
        agent.destination = agent.transform.position;
        agent.speed = 0f;
    }
}
