using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class HealPlayerOverTime : MonoBehaviour
{

    public float HealPerSecond = 0f;

    public float SecondsActive = 0f;

    private float secondsRemaining = 0f;


    public void Start()
    {

        secondsRemaining = SecondsActive;
    }
    // Update is called once per frame
    public void Update()
    {
        if (secondsRemaining > 0f)
        {
            GetComponent<EllenHealthScript>().HealPlayer(HealPerSecond * Time.deltaTime);
            secondsRemaining -= Time.deltaTime;
        }
        else
        {
            Destroy(this);
        }
    }
}
