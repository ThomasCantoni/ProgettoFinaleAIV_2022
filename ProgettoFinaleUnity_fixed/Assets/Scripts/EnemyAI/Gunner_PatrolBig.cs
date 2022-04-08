using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Gunner_PatrolBig : Enemy_PatrolBig
{
    private float speed = 4f;
    private float acceleration = 2f;

    int count = -1;
    bool turnBack = false;
    float timer;

    protected NavMeshAgent agent;
    protected GunnerBigSM sm;
    public Gunner_PatrolBig(GunnerBigSM stateMachine) : base("Gunner_PatrolBig", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        
        sm.DetectCollider.enabled = true;
        sm.OnShpereTriggerStay += OnDetection;
        sm.AttackCollider.enabled = false;
        agent = sm.gameObject.GetComponent<NavMeshAgent>();
        Debug.Log(agent);
        CalculateCount();
        agent.speed = speed;
        agent.destination = sm.patrolPoints[count].position;
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
            if (count >= 0 && count < sm.patrolPoints.Count)
                agent.destination = sm.patrolPoints[count].position;
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
