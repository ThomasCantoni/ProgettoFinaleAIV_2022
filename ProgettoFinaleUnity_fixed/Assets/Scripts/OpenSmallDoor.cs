using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSmallDoor : MonoBehaviour
{
    public Animator Anim;
    private void OnTriggerStay(Collider collision)
    {
            
                Anim.SetBool("Open", true);
                Anim.SetTrigger("OpenDoor");
            
    }
    private void OnTriggerExit(Collider collision)
    {
            
                Anim.SetTrigger("CloseDoor");
                Anim.SetBool("Open", false);
            
    }
}
