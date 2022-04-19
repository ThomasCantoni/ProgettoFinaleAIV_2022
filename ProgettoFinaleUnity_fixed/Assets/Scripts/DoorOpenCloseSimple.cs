using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenCloseSimple : MonoBehaviour
{
    Animator Anim;
    private void Start()
    {
        Anim = transform.parent.GetComponent<Animator>();
    }
    public void OpenDoor()
    {
        Anim.SetBool("isDoorOpen", true);
        Anim.SetTrigger("OpenDoor");
    }
    public void CloseDoor()
    {
        Anim.SetBool("isDoorOpen", true);
        Anim.SetTrigger("CloseDoor");
    }
}
