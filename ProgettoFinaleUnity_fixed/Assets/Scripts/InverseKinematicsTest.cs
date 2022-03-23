using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class InverseKinematicsTest : MonoBehaviour
{
   
    public bool GunEquipped = false;
    public Rig Rig;
    public TwoBoneIKConstraint bones;
    public Transform TargetIKShooting;
    public float target=0f;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PlayerControllerSecondVersion>().controls.Player.Shot.performed+= ManageShooting;
        GetComponent<PlayerControllerSecondVersion>().controls.Player.EquipWeapon.performed += EquipGun;

    }
    void ManageShooting(InputAction.CallbackContext ctx)
    {
        
        if(GunEquipped)
        {
            bones.data.target = TargetIKShooting;
            Shoot();
        }
        else
        {
            GunEquipped = true;
            target = 1f;
            
        }
    }
    void EquipGun(InputAction.CallbackContext ctx)
    {
        if(GunEquipped)
        {
            GunEquipped = false;
            GetComponent<PlayerControllerSecondVersion>().Anim.SetBool("PutAwayGun", true);
            GetComponent<PlayerControllerSecondVersion>().Anim.SetBool("GunEquipped", false);



            target = 0f;
        }
        else
        {
            GunEquipped = true;
            GetComponent<PlayerControllerSecondVersion>().Anim.SetBool("GunEquipped", true);
           

            target = 1f;
        }
    }
    private void Update()
    {
        if (!GetComponent<PlayerControllerSecondVersion>().isAiming)
        {
            Rig.weight = Mathf.Lerp(Rig.weight, target, 0.2f);
           
        }
        else
        {
            Rig.weight = 0f;
        }
        
    }
    void Shoot()
    {

    }
}
