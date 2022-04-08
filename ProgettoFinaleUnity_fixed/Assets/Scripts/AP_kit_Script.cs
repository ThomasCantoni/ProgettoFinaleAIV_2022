using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AP_kit_Script : MonoBehaviour
{
    public float ApReplenished = 30f;


    public AudioClip clipToPlay;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && other.GetComponent<EllenActionPoints>().AP_Value < other.GetComponent<EllenActionPoints>().MaxAp)
        {

            other.GetComponent<EllenActionPoints>().AP_Value += ApReplenished;

            if (clipToPlay == null)
            {
                Destroy(this.gameObject);
            }
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            this.GetComponent<AudioSource>().spatialBlend = 0f;
            this.GetComponent<AudioSource>().volume = 0.2f;

            this.GetComponent<AudioSource>().pitch = 1f;

            this.GetComponent<AudioSource>().clip = clipToPlay;
            this.GetComponent<AudioSource>().loop = false;
            this.GetComponent<AudioSource>().Play();
            Destroy(this.gameObject, clipToPlay.length);
        }
    }
}
