using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;
using Cinemachine;


public class EllenHealthScript : MonoBehaviour
{
    public Image HP_Image;
    public Animator anim;
    private float hp_Value = 100f;
    private float maxHp = 100f;
    public Canvas canvasDeath;
    public Canvas UiCanvas;


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

    public IEnumerator activateCanvas()
    {
        yield return new WaitForSeconds(2.5f);
        canvasDeath.gameObject.SetActive(true);
        UiCanvas.gameObject.SetActive(false);
        
    }


    public void DamagePlayer(float amount)
    {
        if (hp_Value <= 0)
        {
            return;
        }
        HP_Value -= amount;

        if (hp_Value <= 0)
        {
            anim.SetBool("isDeath", true);
            anim.SetTrigger("EllenDeath");
            PlayerControllerSecondVersion PCSV = GetComponent<PlayerControllerSecondVersion>();
            PCSV.controls.asset.Disable();
            StartCoroutine(activateCanvas());
            GetComponent<CharacterController>().enabled = false;
            GetComponent<RigBuilder>().enabled = false;
            this.gameObject.AddComponent<CameraOut>();
            
            this.gameObject.GetComponent<CameraOut>().cam = PCSV.ThirdPersonCamera;
           
        }
    }
    public void HealPlayer(float amount)
    {
        HP_Value += amount;
    }

}
