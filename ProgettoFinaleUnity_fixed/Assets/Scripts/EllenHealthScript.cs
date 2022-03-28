using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EllenHealthScript : MonoBehaviour
{
    public Image HP_Image;
    private float hp_Value = 100f;
    private float maxHp = 100f;
    public float HP_Value 
    { 
        get
        {
            return hp_Value;
        }
        set
        {
            hp_Value = Mathf.Clamp(value, -maxHp, maxHp);
            HP_Image.GetComponent<Slider>().value = hp_Value;
        }
    }

    public void DamagePlayer(float amount)
    {
        HP_Value -= amount;
    }
    public void HealPlayer(float amount)
    {
        HP_Value += amount;
    }

}
