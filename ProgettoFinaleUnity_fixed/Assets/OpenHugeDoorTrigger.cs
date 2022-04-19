using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenHugeDoorTrigger : MonoBehaviour
{
    public Animator Anim;
    public bool RequiresKey;
    public int KeyRequired;
  
    private void OnTriggerStay(Collider collision)
    {
        
        if(RequiresKey)
        {
            if(!collision.GetComponent<PlayerControllerSecondVersion>().PlayerData.keysTaken.Contains(KeyRequired))
            {
                return;
            }
        }
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
