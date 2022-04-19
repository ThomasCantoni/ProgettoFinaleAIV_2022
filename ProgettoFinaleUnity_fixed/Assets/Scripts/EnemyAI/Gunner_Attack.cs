using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Gunner_Attack : Enemy_Attack
{
    private float timer = 0f;
    private AnimatorStateInfo infoAnim;
    private AnimatorTransitionInfo infoTrans;
    private int animRangeAttackStateHash = Animator.StringToHash("Grenadier_RangeAttack");
    private int animEndRangeTransitionStateHash = Animator.StringToHash("Grenadier_RangeAttack -> Grenadier_Idle");
    private int animStartRangeTransStateHash = Animator.StringToHash("Grenadier_Idle -> Grenadier_RangeAttack");

    private int animMeleeAttackStateHash = Animator.StringToHash("Grenadier_MeleeAttack");
    private int animEndMeleeTransitionStateHash = Animator.StringToHash("Grenadier_MeleeAttack -> Grenadier_Idle");
    private int animStartMeleeTransStateHash = Animator.StringToHash("Grenadier_Idle -> Grenadier_MeleeAttack");
    private RaycastHit hitInfo;

    GunnerSM sm;
    public Gunner_Attack(GunnerSM stateMachine) : base("Gunner_Attack", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        timer = 0f;
        sm.DetectCollider.enabled = false;
        sm.animAct += SetMeleeAttackCollider;
        sm.animAct += Shoot;
    }

    public override void UpdateLogic()
    {
        infoTrans = sm.anim.GetAnimatorTransitionInfo(0);
        infoAnim = sm.anim.GetCurrentAnimatorStateInfo(0);
        if ((infoAnim.shortNameHash == animRangeAttackStateHash || infoTrans.nameHash == animStartRangeTransStateHash) && (infoTrans.nameHash != animEndRangeTransitionStateHash))
        {
            Physics.Raycast(sm.transform.position + Vector3.up, sm.ObjToChase.position - sm.transform.position, out hitInfo, 50f);
            if (hitInfo.transform != sm.ObjToChase)
            {
                sm.anim.SetTrigger("ObjBehindWall");
            }
        }
        else if (infoAnim.shortNameHash == animMeleeAttackStateHash || infoTrans.nameHash == animStartMeleeTransStateHash)
        {

        }
        else
        {
            Physics.Raycast(sm.transform.position + Vector3.up, sm.ObjToChase.position - sm.transform.position, out hitInfo, 50f);
            if (hitInfo.transform != sm.ObjToChase)
            {
                sm.ChangeState(sm.chaseState);
            }

            if (Vector3.Distance(sm.transform.position, sm.ObjToChase.position) >= (sm.AttackDistance + 2))
            {
                sm.ChangeState(sm.chaseState);
            }
            timer += Time.deltaTime;
            if (timer >= sm.PreAttackCooldown)
            {
                timer = 0f;
                if (Vector3.Distance(sm.transform.position, sm.ObjToChase.position) >= 5.5f)
                {
                    
                    RangeAttack();
                }
                else
                {
                    
                    MeleeAttack();
                }
            }
        }

        Vector3 dest = (sm.ObjToChase.position - sm.transform.position).normalized;
        sm.transform.forward = Vector3.Lerp(sm.transform.forward, new Vector3(dest.x, 0, dest.z), 0.05f);
    }
    protected virtual void SetMeleeAttackCollider(bool value)
    {
        if ((infoAnim.shortNameHash == animRangeAttackStateHash || infoTrans.nameHash == animStartRangeTransStateHash))
        {
            return;
        }
        sm.AttackCollider.enabled = value;
        sm.OnMeleeAttack(value);
    }

    protected virtual void RangeAttack()
    {
        sm.anim.SetTrigger("RangeAttack");
    }
    protected virtual void MeleeAttack()
    {
        sm.anim.SetTrigger("MeleeAttack");
    }
    protected virtual void Shoot(bool f)
    {
        if (infoAnim.shortNameHash == animMeleeAttackStateHash || infoTrans.nameHash == animStartMeleeTransStateHash)
        {
            return;
        }
        GameObject go = sm.BulletTransform.GetComponent<GunnerBulletPoolMgr>().SpawnObj(sm.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        if (go != null)
        {
            Vector3 velocityOffset;
            Vector3 targetVelocity = Vector3.zero;

            if (sm.UseVeloictyOffset)
            {
                targetVelocity = sm.ObjToChase.GetComponent<CharacterController>().velocity;
                velocityOffset = new Vector3(targetVelocity.x * 0.6f, targetVelocity.y * 0.2f, targetVelocity.z * 0.6f);
            }
            else
            {
                velocityOffset = Vector3.zero;
            }

            Debug.Log(targetVelocity);
            go.transform.LookAt(sm.ObjToChase.position + new Vector3(0, 1, 0) + velocityOffset, Vector3.up);
        }
    }

    public override void OnExit()
    {
        //sm.anim.SetBool("WalkFast", true);
        sm.anim.SetBool("Idle", false);
        sm.animAct -= Shoot;
        sm.animAct -= SetMeleeAttackCollider;
    }
}
