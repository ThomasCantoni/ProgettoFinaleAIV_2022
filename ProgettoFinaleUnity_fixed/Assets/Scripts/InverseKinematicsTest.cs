using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class InverseKinematicsTest : MonoBehaviour
{
    public PlayerControllerSecondVersion PCSV;
    public GameObject Gun;
    [SerializeField] public Rig zoomLookAtRig;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform point;
    public Rig RestingGunRig;
    public Rig LookAtConstraintRIG;
    public TwoBoneIKConstraint restingGun;
    public Transform TargetIKShooting;
    private Transform CameraReference;
    public float restingTarget = 0f;
    public float returnToRestCooldown = 0.5f;
    public float returnToRestCooldownReset = 0.5f;
    public float zoomLookAtTarget = 0f;
 
    public float FireRate 
    { get
        {
            return 1f / shootCooldownReset;
        }
        set
        {
            shootCooldownReset = 1f / value;
        }
    }
    public float shootCooldownReset = 0.6f;
    public float shootCooldown = 0f;
    public bool GunEquipped = false, Shooting = false;
    public bool ShootingAvailable = false;
    public bool ShootingAvailableAngle = false;


    // Start is called before the first frame update
    void Start()
    {
        LookAtConstraintRIG.weight = 0f;
        PCSV = GetComponent<PlayerControllerSecondVersion>();
        CameraReference = GetComponent<PlayerControllerSecondVersion>().CameraReference;
        GetComponent<PlayerControllerSecondVersion>().Controls.Player.Shot.performed += ManageShooting;
        GetComponent<PlayerControllerSecondVersion>().Controls.Player.EquipWeapon.performed += EquipGun;
        GetComponent<PlayerControllerSecondVersion>().Controls.Player.Zoom.performed += SetIKWeights;
        GetComponent<PlayerControllerSecondVersion>().Controls.Player.Zoom.canceled += CancelIKWeights;

        //GetComponent<PlayerControllerSecondVersion>().controls.Player.RotateCamera.performed += CheckInverseKinematics;
        // GetComponent<PlayerControllerSecondVersion>().controls.Player.Movement.performed += CheckInverseKinematics;

    }
    void SetIKWeights(InputAction.CallbackContext ctx)
    {

        if (TimeManager.IsGamePaused || !GunEquipped)
        {
            return;
        }
        zoomLookAtRig.weight = 1f;
        if (GunEquipped)
            restingTarget = 0f;

    }
    void CancelIKWeights(InputAction.CallbackContext ctx)
    {


        if (TimeManager.IsGamePaused)
        {
            return;
        }

        zoomLookAtRig.weight = 0f;

        if (GunEquipped)
            restingTarget = 1f;
    }
    void ManageShooting(InputAction.CallbackContext ctx)
    {
        if (TimeManager.IsGamePaused)
        {
            return;
        }
        if (GunEquipped)
        {

            if (ShootingAvailable && ShootingAvailableAngle)
            {

                GetComponent<PlayerControllerSecondVersion>().Anim.SetBool("Shot", true);
                Shooting = true;
                returnToRestCooldown = returnToRestCooldownReset;
                restingTarget = 0f;
                ShootingAvailable = false;
                shootCooldown = shootCooldownReset;
                Shoot();

            }

        }
        else
        {
            GunEquipped = true;
            Gun.SetActive(true);
            GetComponent<PlayerControllerSecondVersion>().Anim.SetBool("GunEquipped", true);

            restingTarget = 1f;

        }
    }
    void EquipGun(InputAction.CallbackContext ctx)
    {
        if (TimeManager.IsGamePaused || PCSV.isAiming)
        {
            return;
        }
        if (GunEquipped)
        {
            GunEquipped = false;
            zoomLookAtTarget = 0;
            restingTarget = 0f;
            returnToRestCooldown = returnToRestCooldownReset;
            PCSV.Anim.SetBool("GunEquipped", false);
            PCSV.Anim.SetBool("Shot", false);
            PCSV.Anim.SetBool("PutAwayGun", true);
            PCSV.Anim.SetBool("GunResting", false);

            Gun.SetActive(false);
            Shooting = false;
            shootCooldown = 0;


        }
        else
        {
            GunEquipped = true;

            Gun.SetActive(true);
            GetComponent<PlayerControllerSecondVersion>().Anim.SetBool("GunEquipped", true);
            restingTarget = 1f;
            zoomLookAtTarget = 1f;
        }
    }
    private void Update()
    {
        if (TimeManager.IsGamePaused)
        {
            return;
        }



        if (!Shooting)
        {
            RestingGunRig.weight = Mathf.Lerp(RestingGunRig.weight, restingTarget, 0.2f);
            PCSV.Anim.SetBool("GunResting", true);
            //ShootingGunRig.weight = Mathf.Lerp(ShootingGunRig.weight, shootingTarget, 0.3f);

        }
        else
        {

            RestingGunRig.weight = 0f;
            PCSV.Anim.SetBool("GunResting", false);
            shootCooldown -= Time.deltaTime;

            returnToRestCooldown -= Time.deltaTime;
            zoomLookAtTarget = 1f;

        }
        if (shootCooldown <= 0f)
        {
            ShootingAvailable = true;
            

        }

        if (returnToRestCooldown <= 0f)
        {
            returnToRestCooldown = returnToRestCooldownReset;

            GetComponent<PlayerControllerSecondVersion>().Anim.SetBool("Shot", false);
            if (!PCSV.isAiming)
                restingTarget = 1f;

            Shooting = false;
        }
        CheckLookAtAngle();
    }
    void CheckLookAtAngle()
    {
        float angleRadians = Mathf.Acos(Vector3.Dot(this.transform.forward, CameraReference.forward));
        angleRadians *= 180f / 3.14f;
        //angleRadians = Mathf.Abs(angleRadians);

        if (angleRadians > 105f)
        {
            ShootingAvailableAngle = false;
            LookAtConstraintRIG.weight = Mathf.Lerp(LookAtConstraintRIG.weight, 0f, 0.1f);
        }
        else
        {
            ShootingAvailableAngle = true;

            LookAtConstraintRIG.weight = Mathf.Lerp(LookAtConstraintRIG.weight, 0.9f, 0.1f);

        }
    }
    void Shoot()
    {

        PCSV.Anim.SetBool("Shot", true);
        Vector2 screenCenterPoint = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Ray ray = PCSV.Camera.ScreenPointToRay(screenCenterPoint);
        GunScript GS = Gun.GetComponent<GunScript>();
        GS.FX_Play();
        // Physics.SphereCast(ray, 0.05f, out RaycastHit info, 100f, aimColliderLayerMask);
        if (Physics.SphereCast(ray,0.1f, out RaycastHit hit, 100f, aimColliderLayerMask)) // if the object i hit is an enemy
        {
            HittableType type = HittableType.Other;
            if (hit.collider.GetComponent<IHittable>() != null)
            {
                type = hit.collider.GetComponent<IHittable>().OnHit(GetComponent<Collider>());
            }
            Gun.GetComponent<GunScript>().ReceiveShotImpactPos(hit.point, hit.normal, type);
        }
        else
        {

        }
    }

}
