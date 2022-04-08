using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedCollider : MonoBehaviour
{
    public PlayerControllerSecondVersion PCSV;
    private void Start()
    {
        //PCSV = gameObject.GetComponentInParent<PlayerControllerSecondVersion>();
    }
    private void OnTriggerStay(Collider other)
    {

        PCSV.isGrounded = true;
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
    }
    private void OnTriggerEnter(Collider other)
    {
        PCSV.isGrounded = true;
    }
}
