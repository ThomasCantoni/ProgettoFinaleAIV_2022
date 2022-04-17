using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioDeath : MonoBehaviour
{
    public AudioSource AS;
    public AudioClip Clip,Clip2;
    // Start is called before the first frame update
    void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    public void PlaySourceDeath()
    {
        if (Clip != null)
        {
            AS.clip = Clip;
        }
        AS.Play();
    }
    public void PlaySource2()
    {
        if (Clip != null)
        {
            AS.clip = Clip2;
        }
        AS.Play();
    }
}
