using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossDoorScript : MonoBehaviour
{
    public AudioSource AS;
    public Transform LeftRock, RightRock, CenterRock;
    public int ActivatedKeys;
    public List<int> KeysID;
    public float StopAudioTimer = 3f;
    public void ActivateKey(int ID)
    {
        if(!KeysID.Contains(ID))
        {
            KeysID.Add(ID);
            ActivatedKeys++;

        }
        if (ActivatedKeys >= 2)
        {
            //this.GetComponent<Animator>().SetTrigger("Open");
            LeftRock.GetComponent<Animator>().SetTrigger("Open");
            RightRock.GetComponent<Animator>().SetTrigger("Open");

            CenterRock.GetComponent<Animator>().SetTrigger("Open");

            AS.Play();
            Destroy(AS, StopAudioTimer);
        }
    }
  
}
