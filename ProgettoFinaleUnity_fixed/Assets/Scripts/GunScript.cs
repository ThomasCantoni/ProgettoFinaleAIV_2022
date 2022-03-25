using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    PlayerControllerSecondVersion PCSV;
    public Transform gunMuzzle;
    // Start is called before the first frame update
    void OnEnable()
    {
        PCSV = GetComponent<PlayerControllerSecondVersion>();
    }
    public void ReceiveShotImpactPos(Vector3 impactPos)
    {
        Vector3 towardsImpactPoint = gunMuzzle.position - impactPos;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
