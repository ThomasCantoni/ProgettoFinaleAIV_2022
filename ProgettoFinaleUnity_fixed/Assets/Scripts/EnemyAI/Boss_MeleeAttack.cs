using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_MeleeAttack : BaseState
{
    private int meleeAttackStateHash = Animator.StringToHash("Boss_MeleeAttack");
    private int meleeAttackEndTransHash = Animator.StringToHash("Boss_MeleeAttack -> Boss_Idle");
    private int meleeAttackStartTransHash = Animator.StringToHash("Boss_Idle -> Boss_MeleeAttack");

    BossSM sm;
    public Boss_MeleeAttack(BossSM stateMachine) : base("Boss_MeleeAttack", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        sm.animAct += SetMeleeAttackCollider;
        sm.anim.SetTrigger("MeleeAttack");
        sm.ChangeColor(1);
    }

    public override void UpdateLogic()
    {
        sm.GetCurrentAnimatorInfo(0);
        if (sm.TransInfo.nameHash == meleeAttackEndTransHash)
            sm.ChangeState(sm.cooldownState);
    }

    public override void OnExit()
    {
        sm.animAct -= SetMeleeAttackCollider;
        sm.ChangeColor(0);
    }

    protected virtual void SetMeleeAttackCollider(bool value)
    {
        sm.MeleeAttackCollider.enabled = value;
        if (value)
        {
            sm.OnMeleeAttackStart();
        }
    }
}
