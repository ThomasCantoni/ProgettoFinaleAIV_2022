using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class InverseKinematicsTest : MonoBehaviour
{
   
    public bool GunEquipped = false;
    public Rig Rig;
    public TwoBoneIKConstraint restingGun,shootingGun;
    public Rig LookAtConstraintRIG;
    public Transform TargetIKShooting;
    private Transform CameraReference;
    public float target=0f;
    // Start is called before the first frame update
    void Start()
    {
        CameraReference = GetComponent<PlayerControllerSecondVersion>().CameraReference;
        GetComponent<PlayerControllerSecondVersion>().controls.Player.Shot.performed += ManageShooting;
        GetComponent<PlayerControllerSecondVersion>().controls.Player.EquipWeapon.performed += EquipGun;
        GetComponent<PlayerControllerSecondVersion>().controls.Player.RotateCamera.performed += CheckInverseKinematics;
        GetComponent<PlayerControllerSecondVersion>().controls.Player.Movement.performed += CheckInverseKinematics;

    }
    void ManageShooting(InputAction.CallbackContext ctx)
    {
        
        if(GunEquipped)
        {
            restingGun.data.target.transform.position = TargetIKShooting.position;
            
            
            Shoot();
        }
        else
        {
            GunEquipped = true;
            GetComponent<PlayerControllerSecondVersion>().Anim.SetBool("GunEquipped", true);

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
    void CheckInverseKinematics(InputAction.CallbackContext ctx)
    {
        float angleRadians = Vector3.Dot(this.transform.forward, CameraReference.forward);
        angleRadians *= 180f / 3.14f;
        //angleRadians = Mathf.Abs(angleRadians);
        Debug.Log(angleRadians);
        if (angleRadians < -50f)
        {
            LookAtConstraintRIG.weight = Mathf.Lerp(LookAtConstraintRIG.weight, 0f, 0.1f);
        }
        else
        {
            LookAtConstraintRIG.weight = Mathf.Lerp(LookAtConstraintRIG.weight, 0.75f, 0.1f);

        }
    }
    void Shoot()
    {

    }
}
