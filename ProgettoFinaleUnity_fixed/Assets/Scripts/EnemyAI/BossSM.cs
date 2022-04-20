using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class BossSM : StateMachine
{
    public delegate void ActionAnim(bool start);
    public event ActionAnim animAct;

    public UnityEvent<float, float, float> EllenCameraShake;

    [HideInInspector]
    public Boss_Cooldown cooldownState;
    [HideInInspector]
    public Boss_RangedAttack rangedAttackState;
    [HideInInspector]
    public Boss_SpawnAttack spawnAttackState;
    [HideInInspector]
    public Boss_MeleeAttack meleeAttackState;
    [HideInInspector]
    public Boss_Death deathState;
    [HideInInspector]
    public Transform Player;

    public Animator anim;
    public NavMeshAgent agent;
    public Transform BulletTransform;
    public GameObject BulletPrefab;
    public int NumBullets;
    public Transform SpitterTransform;
    public GameObject SpitterPrefab;
    public int NumSpitters;
    public Transform[] SpitterSpawnPositions;
    public float AttackDistance = 3f;
    public float AttackCooldown = 1f;
    public Collider MeleeAttackCollider;
    public Collider BodyCollider;
    public Material EyesMat;
    public Material CoreMat;
    [ColorUsage(true, true)]
    public Color[] Colors;

    [HideInInspector]
    public AnimatorStateInfo AnimStateInfo;
    [HideInInspector]
    public AnimatorTransitionInfo TransInfo;

    public GameObject MeleeAttackEffect;

    [HideInInspector]
    public bool ReachedHealthThreshold;

    void Awake()
    {
        OnAwake();
    }

    public void ChangeColor(int value)
    {
        EyesMat.SetColor("_EmissionColor", Colors[value]);
        CoreMat.color = Colors[value];
    }

    public void GetCurrentAnimatorInfo(int layer)
    {
        AnimStateInfo = anim.GetCurrentAnimatorStateInfo(layer);
        TransInfo = anim.GetAnimatorTransitionInfo(layer);
    }

    public void HandleAttackAnim(bool start)
    {
        animAct?.Invoke(start);
    }

    public void OnEnemyDeath()
    {
        ChangeState(deathState);
    }

    protected override BaseState GetInitialState()
    {
        return cooldownState;
    }

    protected virtual void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();
        cooldownState = new Boss_Cooldown(this);
        rangedAttackState = new Boss_RangedAttack(this);
        meleeAttackState = new Boss_MeleeAttack(this);
        spawnAttackState = new Boss_SpawnAttack(this);
        deathState = new Boss_Death(this);

        InstantiateBullets();
        InstantiateSpitters();
    }

    protected virtual void InstantiateBullets()
    {
        for (int i = 0; i < NumBullets; i++)
        {
            GameObject go = Instantiate(BulletPrefab);
            go.SetActive(false);
            go.transform.parent = BulletTransform;
            if (BulletTransform.GetComponent<GunnerBulletPoolMgr>() == null)
            {
                BulletTransform.gameObject.AddComponent<GunnerBulletPoolMgr>();
                BulletTransform.GetComponent<GunnerBulletPoolMgr>().OnCreation();
            }
            BulletTransform.GetComponent<GunnerBulletPoolMgr>().AddBullet(go);
        }
    }

    protected virtual void InstantiateSpitters()
    {
        for (int i = 0; i < NumSpitters; i++)
        {
            GameObject go = Instantiate(SpitterPrefab);
            go.SetActive(false);
            go.transform.parent = SpitterTransform;
            go.transform.position = SpitterTransform.position;
            if (SpitterTransform.GetComponent<BossSpitterPoolMgr>() == null)
            {
                SpitterTransform.gameObject.AddComponent<BossSpitterPoolMgr>();
                SpitterTransform.GetComponent<BossSpitterPoolMgr>().OnCreation();
            }
            SpitterTransform.GetComponent<BossSpitterPoolMgr>().AddSpitter(go);
        }
    }

    public virtual void OnMeleeAttackStart()
    {
        GameObject effect = Instantiate(MeleeAttackEffect, transform.position + new Vector3(0, 1f, 0), Quaternion.LookRotation(Vector3.up));
        Destroy(effect, 1.5f);
    }

    public virtual void OnSpawnAttackStart()
    {
        EllenCameraShake?.Invoke(1.5f, 2f, 2f);
    }

    public virtual void OnPlayerDeath()
    {
        anim.SetTrigger("PlayerDeath");
        ChangeState(cooldownState);
        SpitterTransform.GetComponent<BossSpitterPoolMgr>().KillAllSpitters();
        InstantiateSpitters();
        AttackCooldown = 1f;
        this.enabled = false;
    }

    public virtual void OnHealthThreshold(bool trigger50Percent)
    {
        ReachedHealthThreshold = true;
        if (trigger50Percent)
        {
            AttackCooldown = AttackCooldown * 0.5f;
        }
    }
}
