using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chomper_Attack : Enemy_Attack
{
    private float timer = 0f;
    private bool hasAttacked = false;
    private AnimatorStateInfo infoAnim;
    private AnimatorTransitionInfo infoTrans;
    private int animAttacckStateHash = Animator.StringToHash("Chomper_Attack");
    private int animEndTransitionStateHash = Animator.StringToHash("Chomper_Attack -> Cooldown");
    private int animStartTransStateHash = Animator.StringToHash("Cooldown -> Chomper_Attack");

    ChomperSM sm;
    public Chomper_Attack(ChomperSM stateMachine) : base("Chomper_Attack", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        timer = 0f;
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
                sm.agent.transform.position = sm.anim.transform.position;
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
    protected override void Attack()
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
        sm.animAct -= SetAttackCollider;
    }
}
