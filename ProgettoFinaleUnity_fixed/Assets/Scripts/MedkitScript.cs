using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct HealOverTimeData
{
    public float HealPerSecond;

    public float SecondsActive;


}
public class MedkitScript : MonoBehaviour
{
    public float InstantHealAmount = 15f;
    public bool ApplyHealOverTime = false;
    [SerializeField]
    HealOverTimeData DataToApply = new HealOverTimeData();
    public AudioClip clipToPlay;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && other.GetComponent<EllenHealthScript>().HP_Value < 100f)
        {

            other.GetComponent<EllenHealthScript>().HealPlayer(InstantHealAmount);
            if (ApplyHealOverTime)
            {
                HealPlayerOverTime HPOT = other.gameObject.AddComponent<HealPlayerOverTime>();
                HPOT.HealPerSecond = DataToApply.HealPerSecond;
                HPOT.SecondsActive = DataToApply.SecondsActive;
            }
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
