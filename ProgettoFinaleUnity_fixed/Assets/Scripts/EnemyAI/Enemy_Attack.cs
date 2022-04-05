using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Attack : BaseState
{
    NavMeshAgent agent;
    private float timer = 0.1f;
    private bool hasAttacked = false;
    private float speed = 2f;
    private float acceleration = 2f;
    
    ChomperSM sm;
    public Enemy_Attack(ChomperSM stateMachine) : base("Enemy_Patrol", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        agent = sm.gameObject.GetComponent<NavMeshAgent>();
        hasAttacked = false;
        agent.destination = sm.ObjToChase.position;
        sm.AttackCollider.enabled = true;
        sm.DetectCollider.enabled = false;

        sm.OnShpereTriggerEnter += OnAttackSuccess;
    }

    public override void UpdateLogic()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && !hasAttacked)
        {
            hasAttacked = true;
            Attack();
        }
        else
        {
            sm.transform.LookAt(sm.ObjToChase, Vector3.up);
        }
    }

    protected virtual void Attack()
    {
        Debug.Log("ATTACK");
        sm.anim.SetTrigger("Attack");
    }

    protected virtual void OnAttackSuccess(GameObject sender, Collider collider, string message, bool fromEvent)
    {
        //HANDLE PLAYER DAMAGE
        Debug.Log("PLAYER HITTED");
    }

    public override void OnExit()
    {
        sm.anim.SetBool("Run", false);
        agent.speed = speed;
        sm.OnShpereTriggerEnter -= OnAttackSuccess;
    }
}
