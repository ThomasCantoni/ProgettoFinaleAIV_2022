using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Attack : BaseState
{
    NavMeshAgent agent;

    private float timer = 0f;
    private bool hasAttacked = false;
    private float speed = 2f;
    private float acceleration = 2f;
    private AnimatorStateInfo infoAnim;
    private AnimatorTransitionInfo infoTrans;
    private int animAttacckStateHash = Animator.StringToHash("Chomper_Attack");
    private int animEndTransitionStateHash = Animator.StringToHash("Chomper_Attack -> Cooldown");
    private int animStartTransStateHash = Animator.StringToHash("Cooldown -> Chomper_Attack");
    
    ChomperSM sm;
    public Enemy_Attack(ChomperSM stateMachine) : base("Enemy_Patrol", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        timer = 0f;
        agent = sm.gameObject.GetComponent<NavMeshAgent>();
        hasAttacked = false;
        sm.DetectCollider.enabled = false;
        sm.animAct += SetAttackCollider;
    }

    public override void UpdateLogic()
    {
        
        infoTrans = sm.anim.GetAnimatorTransitionInfo(0);
        infoAnim = sm.anim.GetCurrentAnimatorStateInfo(0);
        if (infoAnim.shortNameHash == animAttacckStateHash || infoTrans.nameHash == animStartTransStateHash)
        {
            if (infoTrans.nameHash == animEndTransitionStateHash)
            {
                agent.transform.position = sm.anim.transform.position;
                sm.anim.transform.localPosition = Vector3.zero;
            }
            else
            {
                sm.anim.applyRootMotion = true;
            }
        }
        else
        {
            sm.anim.applyRootMotion = false;
            if (hasAttacked)
            {
                sm.ChangeState(sm.chaseState);
            }
        }
        
        timer += Time.deltaTime;
        if (timer >= sm.PreAttackCooldown && !hasAttacked)
        {
            hasAttacked = true;
            Attack();
        }
        else if (timer < sm.PreAttackCooldown && !hasAttacked)
        {
            Vector3 dest = (sm.ObjToChase.position - sm.transform.position).normalized;
            sm.transform.forward = new Vector3(dest.x, 0, dest.z);
        }
    }
    protected virtual void Attack()
    {
        sm.anim.SetBool("Idle", false); 
        sm.anim.SetTrigger("Attack");
    }

    protected virtual void SetAttackCollider(bool Active)
    {
        sm.AttackCollider.enabled = Active;

        sm.BodyCollider.enabled = !Active;
    }

    public override void OnExit()
    {
        sm.anim.SetBool("Run", false);
        agent.speed = speed;
        sm.animAct -= SetAttackCollider;
    }
}
