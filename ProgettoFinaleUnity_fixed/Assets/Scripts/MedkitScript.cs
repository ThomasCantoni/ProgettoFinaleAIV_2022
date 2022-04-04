using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitScript : MonoBehaviour
{
    public float HealAmount = 15f;
    void Update()
    {
        this.transform.Rotate(new Vector3(0, 90f * Time.deltaTime, 0));
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3 && other.GetComponent<EllenHealthScript>().HP_Value < 100f)
        {
            
            other.GetComponent<EllenHealthScript>().HealPlayer(HealAmount);
            Destroy(this.gameObject);
        }
    }
}
