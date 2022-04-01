using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenHugeDoorTrigger : MonoBehaviour
{
    public Animator Anim;
    //public MeshCollider mesh;

    private void OnTriggerStay(Collider collision)
    {
        PlayerControllerSecondVersion attempt = new PlayerControllerSecondVersion();

        if (collision.gameObject.TryGetComponent(out attempt))
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
        PlayerControllerSecondVersion attempt = new PlayerControllerSecondVersion();

        if (collision.gameObject.TryGetComponent(out attempt))
        {
            if (Anim.GetBool("isDoorOpen"))
            {
                Anim.SetTrigger("CloseDoor");
                Anim.SetBool("isDoorOpen", false);
            }
        }
    }

}
