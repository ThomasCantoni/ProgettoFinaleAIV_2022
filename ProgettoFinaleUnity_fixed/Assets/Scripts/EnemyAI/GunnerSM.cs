using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class GunnerSM : EnemySM
{
    public Transform BulletTransform;
    public GameObject BulletPrefab;
    public int NumBullets = 3;
    public bool UseVeloictyOffset = true;
    public GameObject PunchTrailEffect;

    public UnityEvent<int> OnHeal;

    public override void OnAwake()
    {
        chaseState = new Gunner_Chase(this);
        attackState = new Gunner_Attack(this);
        deathState = new Gunner_Death(this);

        InstantiateBullets();
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

    public virtual void OnMeleeAttack(bool start)
    {
        PunchTrailEffect.SetActive(start);
    }
}
