using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedCollider : MonoBehaviour
{
    
    public SphereCollider Small, Big;
    public bool touching;
    private void OnTriggerEnter(Collider other)
    {
        touching = true;
        
    }
    private void OnTriggerStay(Collider other)
    {

        touching = true;
        
    }
    private void OnTriggerExit(Collider other)
    {
        touching = false;
        
    }
    public void Disable()
    {
        SphereCollider[] c = GetComponents<SphereCollider>();
        foreach (SphereCollider x in c)
        {
            x.enabled = false;
        }
        touching = false;
        this.enabled = false;
    }
    public void Enable()
    {
       SphereCollider[] c = GetComponents<SphereCollider>();
       foreach(SphereCollider x in c)
        {
            x.enabled = true;
        }
        this.enabled = true;
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
   
}
