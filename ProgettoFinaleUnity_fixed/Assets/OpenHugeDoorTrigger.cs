using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenHugeDoorTrigger : MonoBehaviour
{
    public Animator Anim;

    private void OnTriggerStay(Collider collision)
    {
        

        if (collision.gameObject.layer == 3)
        {
            if (!Anim.GetBool("isDoorOpen"))
            {
                Anim.SetBool("isDoorOpen", true);
                Anim.SetTrigger("OpenDoor");

            }
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (Anim.GetBool("isDoorOpen"))
            {
                Anim.SetTrigger("CloseDoor");
                Anim.SetBool("isDoorOpen", false);

            }
        }
    }

}
