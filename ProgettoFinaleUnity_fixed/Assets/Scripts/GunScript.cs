using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunScript : MonoBehaviour
{
    public PlayerControllerSecondVersion PCSV;
    public ParticleSystem Ps;
    public GameObject[] BulletImpacts;
    public Transform gunMuzzle;
    public AudioSource AudioSource;
    private void Start()
    {
        this.AudioSource = GetComponent<AudioSource>();
    }
    public void ReceiveShotImpactPos(Vector3 impactPos, Vector3 dir, HittableType type)
    {
        Vector3 towardsImpactPoint = impactPos - gunMuzzle.position;
        
        GameObject impactGO = Instantiate(BulletImpacts[(int)type], impactPos, Quaternion.LookRotation(dir));
        Destroy(impactGO, 2f);
        
    }
    public void FX_Play()
    {
        Ps.gameObject.SetActive(true);
        AudioSource.pitch = Random.Range(0.7f, 1.3f);
        Ps.Play();
        AudioSource.Play();
    }
}
