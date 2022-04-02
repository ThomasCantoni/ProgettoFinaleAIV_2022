using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EllenActionPoints : MonoBehaviour
{
    public Image Ap_Image;
    public Image IconFill;
    private float Ap_Value = 100f;
    private float maxAp = 7f;
    private float Ap_Decrement = 1f;
    private float Ap_Increment = 0.5f;
    public bool isActive = false;
    public float Cooldown = 0;
    private float CooldownReset = 3f;

    public float AP_Value 
    { 
        get
        {
            return Ap_Value;
        }
        set
        {
            Ap_Value = Mathf.Clamp(value, 0, maxAp) ;
            Ap_Image.GetComponent<Slider>().value = Ap_Value;
        }
    }
    private void Update()
    {
        if (isActive)
        {
            AP_Value -= Ap_Decrement * Time.unscaledDeltaTime;
        }
        else
        {
            if (Cooldown <= 0)
            {
                AP_Value += Ap_Increment * Time.unscaledDeltaTime;
            }
            else
            {
                Cooldown -= Time.unscaledDeltaTime;
                Cooldown = Mathf.Clamp(Cooldown, 0f, CooldownReset);
                IconFill.fillAmount = (CooldownReset - Cooldown) / CooldownReset;
            }
        }
    }
    public void Activate()
    {
        isActive = true;
    }
    public void Disable()
    {
        Cooldown = CooldownReset;
        isActive = false;

    }
}
