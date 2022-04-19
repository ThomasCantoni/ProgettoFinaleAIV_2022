using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spitter_PatrolBig : Enemy_PatrolBig
{
    private float speed = 2f;

    private int count = -1;
    private bool turnBack = false;
    private float timer;
    private bool firstFrame = true;

    protected SpitterBigSM sm;
    public Spitter_PatrolBig(SpitterBigSM stateMachine) : base("Spitter_PatrolBig", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        sm.ChangeStateText("PATROL");

        firstFrame = true;

        sm.DetectCollider.enabled = true;
        sm.OnShpereTriggerStay += OnDetection;
        sm.AttackCollider.enabled = false;
        CalculateCount();
        sm.agent.speed = speed;
        sm.agent.destination = sm.patrolPoints[count].position;
    }

    public override void UpdateLogic()
    {
        if (!sm.agent.hasPath)
        {
            timer += Time.deltaTime;
            if (timer < sm.TimeBetweenPatrolPoints)
            {
                return;
            }
            timer = 0;
            CalculateCount();
            if (count >= 0 && count < sm.patrolPoints.Count)
                sm.agent.destination = sm.patrolPoints[count].position;
        }
        else if (firstFrame)
        {
            sm.agent.destination = sm.patrolPoints[count].position;
            firstFrame = false;
        }
    }

    public override void OnExit()
    {
        sm.OnShpereTriggerStay -= OnDetection;
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

    public override void OnDetection(GameObject sender, Collider c, string message, bool fromEvent)
    {
        sm.ObjToChase = c.gameObject.transform;
        if (fromEvent)
        {
            foreach (var follower in sm.Followers)
            {
                if (follower.gameObject.activeSelf)
                    follower.patrolState.OnDetection(sender, c, message, false);
            }
        }
        sm.ChangeState(sm.chaseState);
    }
}
