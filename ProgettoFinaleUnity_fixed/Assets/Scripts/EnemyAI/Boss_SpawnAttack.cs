using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SpawnAttack : BaseState
{
    private int spawnAttackStateHash = Animator.StringToHash("Boss_SpawnAttack");
    private int spawnAttackEndTransHash = Animator.StringToHash("Boss_SpawnAttack -> Boss_Idle");
    private int spawnAttackStartTransHash = Animator.StringToHash("Boss_Idle -> Boss_SpawnAttack");

    BossSM sm;
    public Boss_SpawnAttack(BossSM stateMachine) : base("Boss_SpawnAttack", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        sm.animAct += Spawn;
        sm.anim.SetTrigger("SpawnAttack");
        sm.ChangeColor(3);
    }

    public override void UpdateLogic()
    {
        sm.GetCurrentAnimatorInfo(0);
        if (sm.TransInfo.nameHash == spawnAttackEndTransHash)
            sm.ChangeState(sm.cooldownState);
    }

    public override void OnExit()
    {
        sm.animAct -= Spawn;
        sm.ChangeColor(0);
    }

    public virtual void Spawn(bool value)
    {
        sm.OnSpawnAttackStart();
        GameObject go = sm.SpitterTransform.GetComponent<BossSpitterPoolMgr>().SpawnObj(sm.SpitterSpawnPosition.position, sm.SpitterSpawnPosition.forward);
        if (go != null)
        {
            SpawnedSpitterSM spitter = go.GetComponent<SpawnedSpitterSM>();
            spitter.ObjToChase = sm.Player;
            spitter.ChangeState(spitter.idleState);
        }
    }
}
