using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_RangedAttack : BaseState
{
    private int rangeAttackStateHash = Animator.StringToHash("Boss_RangeAttack");
    private int rangeAttackEndTransHash = Animator.StringToHash("Boss_RangeAttack -> Boss_Idle");
    private int rangeAttackStartTransHash = Animator.StringToHash("Boss_Idle -> Boss_RangeAttack");

    BossSM sm;
    public Boss_RangedAttack(BossSM stateMachine) : base("Boss_RangedAttack", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        sm.animAct += Shoot;
        sm.anim.SetTrigger("RangeAttack");
        sm.ChangeColor(2);
    }

    public override void UpdateLogic()
    {
        Vector3 dest = (sm.Player.position - sm.transform.position).normalized;
        sm.transform.forward = Vector3.Lerp(sm.transform.forward, new Vector3(dest.x, 0, dest.z), 0.05f);
        sm.GetCurrentAnimatorInfo(0);
        if (sm.TransInfo.nameHash == rangeAttackEndTransHash)
            sm.ChangeState(sm.cooldownState);
    }

    public override void OnExit()
    {
        sm.animAct -= Shoot;
        sm.ChangeColor(0);
    }

    public void Shoot(bool value)
    {
        GameObject go = sm.BulletTransform.GetComponent<GunnerBulletPoolMgr>().SpawnObj(sm.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        if (go != null)
        {
            Vector3 velocityOffset;
            Vector3 targetVelocity = sm.Player.GetComponent<CharacterController>().velocity;
            velocityOffset = new Vector3(targetVelocity.x * 0.6f, targetVelocity.y * 0.2f, targetVelocity.z * 0.6f);

            go.transform.LookAt(sm.Player.position + new Vector3(0, 1, 0) + velocityOffset, Vector3.up);
        }
    }
}
