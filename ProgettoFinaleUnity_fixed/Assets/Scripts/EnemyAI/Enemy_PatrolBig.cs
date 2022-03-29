using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_PatrolBig : Enemy_Patrol
{
    int count = -1;
    bool turnBack = false;
    float timer;

    ChomperBigSM smBig;
    public Enemy_PatrolBig(ChomperBigSM stateMachine) : base("Enemy_PatrolBig", stateMachine)
    {
        smBig = stateMachine;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CalculateCount();
        agent.destination = smBig.patrolPoints[count].position;
    }

    public override void UpdateLogic()
    {
        if (!agent.hasPath)
        {
            timer += Time.deltaTime;
            if (timer < smBig.TimeBetweenPatrolPoints)
            {
                return;
            }
            timer = 0;
            CalculateCount();
            if (count >= 0 && count < smBig.patrolPoints.Count)
                agent.destination = smBig.patrolPoints[count].position;
        }
    }

    private void CalculateCount()
    {
        if (!smBig.Loop)
        {
            if (turnBack)
            {
                count--;
            }
            else
            {
                count++;
            }

            if (count < 0 || count >= smBig.patrolPoints.Count)
            {
                count = Mathf.Clamp(count, 0, smBig.patrolPoints.Count - 1);
                turnBack = !turnBack;
                count += turnBack ? -1 : 1;
            }
        }
        else
        {
            count++;
            if (count >= smBig.patrolPoints.Count)
                count = 0;
        }
    }

    public override void OnDetection(GameObject sender, Collider c, string message, bool fromEvent)
    {
        base.OnDetection(sender, c, message, fromEvent);
        if (fromEvent)
        {
            foreach (var follower in smBig.Followers)
            {
                if (follower.gameObject.activeSelf)
                    follower.patrolState.OnDetection(sender, c, message, false);
            }
        }
        sm.ChangeState(sm.chaseState);
    }
}
