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
    public AudioSource AudioSource;
    private void Start()
    {
        this.AudioSource = GetComponent<AudioSource>();
    }
    public void ReceiveShotImpactPos(Vector3 impactPos, Vector3 dir)
    {
        Vector3 towardsImpactPoint = impactPos - gunMuzzle.position;
        Ps.gameObject.SetActive(true);
        AudioSource.pitch = Random.Range(0.7f, 1.3f);
        Ps.Play();
        AudioSource.Play();
        GameObject impactGO = Instantiate(BulletImpact, impactPos, Quaternion.LookRotation(dir));
        Destroy(impactGO, 2f);
        
    }
}
