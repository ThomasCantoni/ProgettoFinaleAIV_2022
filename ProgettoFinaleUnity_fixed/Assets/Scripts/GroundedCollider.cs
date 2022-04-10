using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedCollider : MonoBehaviour
{
    public PlayerControllerSecondVersion PCSV;
    public SphereCollider Small, Big;
    private void Start()
    {
        //PCSV = gameObject.GetComponentInParent<PlayerControllerSecondVersion>();
    }
    private void OnTriggerStay(Collider other)
    {

        PCSV.isGrounded = true;
        GlobalVariables.IsPlayerGrounded = true;
    }
    public void Disable()
    {
        SphereCollider[] c = GetComponents<SphereCollider>();
        foreach (SphereCollider x in c)
        {
            x.enabled = false;
        }
    }
    public void SwitchToSmall()
    {
        Small.enabled = true;
        Big.enabled = false;
    }
    public void SwitchToBig()
    {
        Small.enabled = false;
        Big.enabled = true;
    }
    public void Enable()
    {
       SphereCollider[] c = GetComponents<SphereCollider>();
       foreach(SphereCollider x in c)
        {
            x.enabled = false;
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    PCSV.isGrounded = true;

    //}
    //private void OnCollisionStay(Collision collision)
    //{

    //    PCSV.isGrounded = true;

    //}
    //private void OnCollisionExit(Collision collision)
    //{
    //    PCSV.isGrounded = false;

    //}
    private void OnTriggerExit(Collider other)
    {
        PCSV.isGrounded = false;
        GlobalVariables.IsPlayerGrounded = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        PCSV.isGrounded = true;
        GlobalVariables.IsPlayerGrounded = true;
    }
}
