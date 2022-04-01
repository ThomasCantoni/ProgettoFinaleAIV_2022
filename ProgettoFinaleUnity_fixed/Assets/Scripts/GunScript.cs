using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunScript : MonoBehaviour
{
    public PlayerControllerSecondVersion PCSV;
    public ParticleSystem Ps;
    public GameObject BulletImpact;
    public Transform gunMuzzle;
    // Start is called before the first frame update
    
    public void ReceiveShotImpactPos(Vector3 impactPos,Vector3 dir)
    {
        Vector3 towardsImpactPoint = impactPos-gunMuzzle.position;
        Ps.gameObject.SetActive(true);
        Ps.Play();
        GameObject impactGO = Instantiate(BulletImpact, impactPos, Quaternion.LookRotation(dir));
        Destroy(impactGO, 2f);
        
    }
}
