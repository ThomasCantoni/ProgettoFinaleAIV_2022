using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioFootsteps : MonoBehaviour
{
    public AudioSource Source;
    public AudioClip[] FootstepClips;

    public AudioClip PlayFootstep()
    {
        int r = Random.Range(0, FootstepClips.Length);
        Source.clip = FootstepClips[r];
        Source.pitch = Random.Range(0.80f, 1.2f);
        Source.Play();
        return FootstepClips[r];
    }
}
