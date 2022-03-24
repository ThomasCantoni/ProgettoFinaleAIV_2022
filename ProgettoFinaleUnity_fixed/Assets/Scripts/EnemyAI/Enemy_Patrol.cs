using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Patrol : BaseState
{
    NavMeshAgent agent;
    int count = -1;
    bool turnBack = false;
    float timer;

    ChomperSM sm;
    public Enemy_Patrol(ChomperSM stateMachine) : base("Enemy_Patrol", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        sm.OnSphereTriggerEnter += OnDetection;
        agent = sm.gameObject.GetComponent<NavMeshAgent>();
        CalculateCount();
        agent.destination = sm.patrolPoints[count].position;

        sm.gameObject.GetComponentInChildren<MeshRenderer>().material = sm.Debug_Materials[0];
    }

    public override void UpdateLogic()
    {
        if (!agent.hasPath)
        {
            timer += Time.deltaTime;
            if (timer < sm.TimeBetweenPatrolPoints)
            {
                return;
            }
            timer = 0;
            CalculateCount();
            agent.destination = sm.patrolPoints[count].position;
        }
    }

    public override void OnExit()
    {
        sm.OnSphereTriggerEnter -= OnDetection;
    }

    private void OnDetection(GameObject sender, Collider c, string message)
    {
        sm.ObjToChase = c.gameObject.transform;
        sm.ChangeState(sm.chaseState);
    }

    private void CalculateCount()
    {
        if (!sm.Loop)
        {
            if (turnBack)
            {
                count--;
            }
            else
            {
                count++;
            }

            if (count < 0 || count >= sm.patrolPoints.Count)
            {
                count = Mathf.Clamp(count, 0, sm.patrolPoints.Count - 1);
                turnBack = !turnBack;
                count += turnBack ? -1 : 1;
            }
        }
        else
        {
            count++;
            if (count >= sm.patrolPoints.Count)
                count = 0;
        }
    }
}
