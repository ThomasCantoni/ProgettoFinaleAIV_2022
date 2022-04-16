using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.UI;

public class Gunner : Enemy 
{
    public void Heal(int amount)
    {
        Health += amount;
        Health = (int)Mathf.Clamp(Health, 1, HP_Slider.maxValue);
        HP_Slider.value = Health;
    }
}
