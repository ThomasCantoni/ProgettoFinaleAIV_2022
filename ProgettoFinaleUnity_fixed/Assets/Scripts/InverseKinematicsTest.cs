using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class InverseKinematicsTest : MonoBehaviour
{
   
    public bool GunEquipped = false,Shooting=false;
    [SerializeField] public Rig zoomLookAtRig;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform point;
    public Rig RestingGunRig,ShootingGunRig;
    public Rig LookAtConstraintRIG;
    public TwoBoneIKConstraint restingGun,shootingGun;
    public Transform TargetIKShooting;
    private Transform CameraReference;
    public float restingTarget = 0f, shootingTarget = 0f, returnToRestCooldown = 1.2f;
    public float zoomLookAtTarget = 0f;

    // Start is called before the first frame update
    void Start()
    {
        CameraReference = GetComponent<PlayerControllerSecondVersion>().CameraReference;
        GetComponent<PlayerControllerSecondVersion>().controls.Player.Shot.performed += ManageShooting; 
        GetComponent<PlayerControllerSecondVersion>().controls.Player.EquipWeapon.performed += EquipGun;
        GetComponent<PlayerControllerSecondVersion>().controls.Player.Zoom.performed += SetIKWeights;
        GetComponent<PlayerControllerSecondVersion>().controls.Player.Zoom.canceled += CancelIKWeights;

        //GetComponent<PlayerControllerSecondVersion>().controls.Player.RotateCamera.performed += CheckInverseKinematics;
        // GetComponent<PlayerControllerSecondVersion>().controls.Player.Movement.performed += CheckInverseKinematics;

    }
    void SetIKWeights(InputAction.CallbackContext ctx)
    {
        zoomLookAtTarget = 1f;
        
    }
    void CancelIKWeights(InputAction.CallbackContext ctx)
    {
        zoomLookAtTarget = 0f;
    }
    void ManageShooting(InputAction.CallbackContext ctx)
    {
        
        if(GunEquipped)
        {
            GetComponent<PlayerControllerSecondVersion>().Anim.SetBool("Shot", true);
            Shooting = true;
            
            restingTarget = 0f;
            
            
            Shoot();
        }
        else
        {
            GunEquipped = true;
            GetComponent<PlayerControllerSecondVersion>().Anim.SetBool("GunEquipped", true);

            restingTarget = 1f;
            
        }
    }
    void EquipGun(InputAction.CallbackContext ctx)
    {
        if(GunEquipped)
        {
            GunEquipped = false;
            restingTarget = 0f;
            returnToRestCooldown = 1.2f;
            GetComponent<PlayerControllerSecondVersion>().Anim.SetBool("PutAwayGun", true);
            GetComponent<PlayerControllerSecondVersion>().Anim.SetBool("GunEquipped", false);
            Shooting = false;


           
        }
        else
        {
            GunEquipped = true;
            GetComponent<PlayerControllerSecondVersion>().Anim.SetBool("GunEquipped", true);
            restingTarget = 1f;
        }
    }
    private void Update()
    {

        if (!Shooting)
        {
            RestingGunRig.weight = Mathf.Lerp(RestingGunRig.weight, restingTarget, 0.2f);
            //ShootingGunRig.weight = Mathf.Lerp(ShootingGunRig.weight, shootingTarget, 0.3f);

        }
        else
        {
            RestingGunRig.weight = 0f;
            returnToRestCooldown -= Time.deltaTime;
            
            //ShootingGunRig.weight = 0f;
        }
        point.position = CameraReference.position + CameraReference.forward*3f;
        zoomLookAtRig.weight = Mathf.Lerp(zoomLookAtRig.weight, zoomLookAtTarget, 0.3f);
        
        if (returnToRestCooldown <= 0f)
        {
            returnToRestCooldown = 1.2f;
            GetComponent<PlayerControllerSecondVersion>().Anim.SetBool("Shot", false);

            restingTarget = 1f;

            Shooting = false;
        }
        CheckLookAtAngle();
    }
    void CheckLookAtAngle()
    {
        float angleRadians = Vector3.Dot(this.transform.forward, CameraReference.forward);
        angleRadians *= 180f / 3.14f;
        //angleRadians = Mathf.Abs(angleRadians);
        Debug.Log(angleRadians);
        if (angleRadians < -55f)
        {
            LookAtConstraintRIG.weight = Mathf.Lerp(LookAtConstraintRIG.weight, 0f, 0.1f);
            //shootingGun.data.target.position = 
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
