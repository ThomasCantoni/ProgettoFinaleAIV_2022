using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySM : StateMachine
{
    public delegate void OnSphereTriggerDelecate(GameObject sender, Collider collider, string message, bool fromEvent);
    public event OnSphereTriggerDelecate OnShpereTriggerStay;
    public event OnSphereTriggerDelecate OnShpereTriggerEnter;

    public delegate void ActionAnim(bool start);
    public event ActionAnim animAct;

    [HideInInspector]
    public Enemy_Patrol patrolState;
    [HideInInspector]
    public Enemy_Chase chaseState;
    [HideInInspector]
    public Enemy_Attack attackState;
    [HideInInspector]
    public Enemy_Death deathState;
    [HideInInspector]
    public Transform ObjToChase;

    public Animator anim;
    public NavMeshAgent agent;
    public float AttackDistance = 3f;
    public float AttackCooldown = 1.5f;
    public float PreAttackCooldown = 0.1f;
    public Collider DetectCollider;
    public Collider AttackCollider;
    public Collider BodyCollider;

    void Awake()
    {
        OnAwake();
    }

    void OnTriggerStay(Collider c)
    {
        OnShpereTriggerStay?.Invoke(this.gameObject, c, "Collision with: " + c.name, true);
    }

    void OnTriggerEnter(Collider c)
    {
        OnShpereTriggerEnter?.Invoke(this.gameObject, c, "Collision with: " + c.name, true);
    }

    public void OnSphereTrigger(Collider c)
    {
        OnShpereTriggerStay?.Invoke(this.gameObject, c, "Hitted by bullet", true);
    }
    public void HandleAttackAnim(bool start)
    {
        animAct?.Invoke(start);
    }
    public void OnEnemyDeath()
    {
        ChangeState(deathState);
    }

    protected virtual void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
}
