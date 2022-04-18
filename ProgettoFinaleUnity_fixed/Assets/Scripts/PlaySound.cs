using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
   public  AudioSource AS;
    public AudioClip Clip,Clip4,Clip3;
    // Start is called before the first frame update
    void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    public void PlaySource()
    {
        if(Clip != null)
        {
            AS.clip = Clip;
        }
        AS.Play();
    }
    public void PlaySource4()
    {
        if (Clip != null)
        {
            AS.clip = Clip4;
        }
        AS.Play();
    }
    public void PlaySource3()
    {
        if (Clip != null)
        {
            AS.clip = Clip3;
        }
        AS.Play();
    }
}
