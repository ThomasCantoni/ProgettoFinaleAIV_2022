using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioDeath : MonoBehaviour
{
    public AudioSource AS;
    public AudioClip Clip;
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
}
