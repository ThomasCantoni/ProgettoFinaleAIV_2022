using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Gunner_Attack : Enemy_Attack
{
    NavMeshAgent agent;

    private float timer = 0f;
    private float speed = 4f;
    private AnimatorStateInfo infoAnim;
    private AnimatorTransitionInfo infoTrans;
    private int animAttacckStateHash = Animator.StringToHash("Grenadier_RangeAttack");
    private int animEndTransitionStateHash = Animator.StringToHash("Grenadier_RangeAttack -> Grenadier_Idle");
    private int animStartTransStateHash = Animator.StringToHash("Grenadier_Idle -> Grenadier_RangeAttack");
    private RaycastHit hitInfo;

    GunnerSM sm;
    public Gunner_Attack(GunnerSM stateMachine) : base("Gunner_Attack", stateMachine)
    {
        sm = stateMachine;
    }

    public override void OnEnter()
    {
        timer = 0f;
        agent = sm.gameObject.GetComponent<NavMeshAgent>();
        sm.DetectCollider.enabled = false;
        sm.DetectMeleeCollider.enabled = false;
        sm.animAct += Shoot;

    }

    public override void UpdateLogic()
    {
        infoTrans = sm.anim.GetAnimatorTransitionInfo(0);
        infoAnim = sm.anim.GetCurrentAnimatorStateInfo(0);
        if ((infoAnim.shortNameHash == animAttacckStateHash || infoTrans.nameHash == animStartTransStateHash) && (infoTrans.nameHash != animEndTransitionStateHash))
        {
            Physics.Raycast(sm.transform.position + Vector3.up, sm.ObjToChase.position - sm.transform.position, out hitInfo, 50f);
            if (hitInfo.transform != sm.ObjToChase)
            {
                sm.anim.SetTrigger("ObjBehindWall");
            }
        }
        else
        {
            Physics.Raycast(sm.transform.position + Vector3.up, sm.ObjToChase.position - sm.transform.position, out hitInfo, 50f);
            if (hitInfo.transform != sm.ObjToChase)
            {
                sm.ChangeState(sm.chaseState);
            }

            //if (Vector3.Distance(sm.transform.position, sm.ObjToChase.position) >= (sm.AttackDistance + 2))
            //{
            //    sm.ChangeState(sm.chaseState);
            //}
            //timer += Time.deltaTime;
            //if (timer >= sm.PreAttackCooldown)
            //{
            //    timer = 0f;
            //    //RangeAttack();
            //}

            if (Vector3.Distance(sm.transform.position, sm.ObjToChase.position) >= (sm.AttackMeleeDistance + 5))
            {
                sm.ChangeState(sm.chaseState);
            }
            timer += Time.deltaTime;
            if (timer >= sm.PreAttackCooldown)
            {
                timer = 0f;
                MeleeAttack();
            }
        }

        Vector3 dest = (sm.ObjToChase.position - sm.transform.position).normalized;
        sm.transform.forward = Vector3.Lerp(sm.transform.forward, new Vector3(dest.x, 0, dest.z), 0.05f);
    }
    protected virtual void RangeAttack()
    {
        sm.anim.SetTrigger("RangeAttack");
    }

    protected virtual void MeleeAttack()
    {
        sm.anim.SetBool("Idle", false);
        sm.anim.SetTrigger("MeleeAttack");
    }
    protected virtual void Shoot(bool f)
    {
        GameObject go = sm.BulletTransform.GetComponent<GunnerBulletPoolMgr>().SpawnObj(sm.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        if (go != null)
        {
            Vector3 velocityOffset;
            Vector3 targetVelocity;

            if (sm.UseVeloictyOffset)
            {
                targetVelocity = sm.ObjToChase.GetComponent<CharacterController>().velocity;
                velocityOffset = new Vector3(targetVelocity.x * 0.6f, targetVelocity.y * 0.2f, targetVelocity.z * 0.6f);
            }
            else
            {
                velocityOffset = Vector3.zero;
            }

            Debug.Log(velocityOffset);
            go.transform.LookAt(sm.ObjToChase.position + new Vector3(0, 1, 0) + velocityOffset, Vector3.up);
        }
    }

    public override void OnExit()
    {
        sm.anim.SetBool("Idle", false);
        agent.speed = speed;
        sm.animAct -= Shoot;
    }
}
