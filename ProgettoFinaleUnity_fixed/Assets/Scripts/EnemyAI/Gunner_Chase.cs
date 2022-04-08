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
    private RaycastHit hitInfo;

    private AnimatorStateInfo infoAnim;
    private AnimatorTransitionInfo infoTrans;
    private int animIdleStateHash = Animator.StringToHash("Grenadier_Idle");
    private int animEndTransitionStateHash = Animator.StringToHash("Grenadier_Idle -> Grenadier_WalkFast");
    private int animStartTransStateHash = Animator.StringToHash("Grenadier_WalkFast -> Grenadier_Idle");


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
        infoTrans = sm.anim.GetAnimatorTransitionInfo(0);
        infoAnim = sm.anim.GetCurrentAnimatorStateInfo(0);
        if (infoAnim.shortNameHash == animIdleStateHash && infoTrans.nameHash != animEndTransitionStateHash)
        {
            agent.speed = 0f;
        }
        else
        {
            agent.speed = speed * acceleration;
        }
        timer += Time.deltaTime;
        if (Vector3.Distance(sm.transform.position, sm.ObjToChase.position) <= sm.AttackDistance )
        {
            Physics.Raycast(sm.transform.position + Vector3.up, sm.ObjToChase.position - sm.transform.position, out hitInfo, 50f);
            if (hitInfo.transform == sm.ObjToChase)
            {
                sm.anim.SetBool("Idle", true);
                sm.ChangeState(sm.attackState);
            }
        }
        if (timer >= 5f)
        {
            sm.ChangeState(sm.patrolState);
        }
        if (Vector3.Distance(sm.transform.position, sm.ObjToChase.position) >= sm.AttackDistance * 2 && timer <= 1f)
        {
            sm.ChangeState(sm.patrolState);
        }
        agent.destination = sm.ObjToChase.position;
    }

    public override void OnExit()
    {
        //METTERE IDLE
        
        sm.anim.SetBool("WalkFast", false);
        agent.destination = agent.transform.position;
        agent.speed = 0f;
    }
}
