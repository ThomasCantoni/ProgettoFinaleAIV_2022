using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossBridgeScript : MonoBehaviour
{
    public AudioSource AS;
    public int ActivatedKeys;
    public List<int> KeysID;
    public void ActivateKey(int ID)
    {
        if(!KeysID.Contains(ID))
        {
            KeysID.Add(ID);
            ActivatedKeys++;

        }
        if (ActivatedKeys >= 3)
        {
            this.GetComponent<Animator>().SetTrigger("Open");
            AS.Play();
        }
    }
  
}
