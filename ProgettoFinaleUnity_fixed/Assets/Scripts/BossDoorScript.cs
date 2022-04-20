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
    public CameraShakeScript Shake;

    public void ActivateKey(int ID)
    {
        if(!KeysID.Contains(ID))
        {
            KeysID.Add(ID);
            ActivatedKeys++;

        }
        if (ActivatedKeys >= 2)
        {
            float amount = 3f;
            //this.GetComponent<Animator>().SetTrigger("Open");
            LeftRock.GetComponent<Animator>().SetTrigger("Open");
            RightRock.GetComponent<Animator>().SetTrigger("Open");
            CenterRock.GetComponent<Animator>().SetTrigger("Open");
            Shake.ApplyShake(amount * 1.2f, amount * 2f, amount * 2f);


            AS.Play();
            Destroy(AS, StopAudioTimer);
        }
    }
  
}
