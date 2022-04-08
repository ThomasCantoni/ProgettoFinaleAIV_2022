using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EllenActionPoints : MonoBehaviour
{
    public Image Ap_Image;
    public Image IconFill;
    private float Ap_Value = 7f;
    public float MaxAp = 7f;
    private float Ap_Decrement = 1f;
    private float Ap_Increment = 0.5f;
    public bool isActive = false;
    public float Cooldown = 0;
    private float CooldownReset = 3f;

    private void Start()
    {
        Cooldown = 0f;
    }
    public float AP_Value
    {
        get
        {
            return Ap_Value;
        }
        set
        {
            Ap_Value = Mathf.Clamp(value, 0, MaxAp);
            Ap_Image.GetComponent<Slider>().value = Ap_Value;
        }
    }

    private void Update()
    {
        if (isActive && !TimeManager.IsGamePaused)
        {
            AP_Value -= Ap_Decrement * Time.unscaledDeltaTime;
        }
        else
        {
            if (Cooldown <= 0)
            {
                AP_Value += Ap_Increment * Time.deltaTime;
            }
            else
            {
                Cooldown -= Time.deltaTime;
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
