using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Cooldown : BaseState
{
    private float cooldownTimer = 0f;

    BossSM sm;
    public Boss_Cooldown(BossSM stateMachine) : base("Boss_Cooldown", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        cooldownTimer = 0f;
        sm.ChangeColor(0);
    }

    public override void UpdateLogic()
    {
        if (sm.Player == null)
        {
            sm.Player = GameObject.Find("Ellen PLAYER").transform;
        }

        Vector3 dest = (sm.Player.position - sm.transform.position).normalized;
        sm.transform.forward = Vector3.Lerp(sm.transform.forward, new Vector3(dest.x, 0, dest.z), 0.05f);

        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= sm.AttackCooldown)
        {
            cooldownTimer = 0f;

            if (sm.SpitterTransform.GetComponent<BossSpitterPoolMgr>().ActiveSpitter < sm.NumSpitters && sm.ReachedHealthThreshold)
            {
                sm.ReachedHealthThreshold = false;
                sm.ChangeState(sm.spawnAttackState);
                return;
            }

            if (Vector3.Distance(sm.Player.position, sm.transform.position) >= sm.AttackDistance)
            {
                sm.ChangeState(sm.rangedAttackState);
            }
            else
            {
                sm.ChangeState(sm.meleeAttackState);
            }
        }
    }

    public override void OnExit()
    {
    }
}
