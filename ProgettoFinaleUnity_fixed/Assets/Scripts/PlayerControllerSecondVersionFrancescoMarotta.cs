using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Animations.Rigging;


public class PlayerControllerSecondVersionFrancescoMarotta : MonoBehaviour
{
    private float PlayerSpeedModifier = 1f;
    private bool isGamePaused = false;
    public float PlayerSpeed
    {
        get
        {
            return Time.deltaTime * PlayerSpeedModifier;
        }
        set
        {
            PlayerSpeedModifier = value;
            Anim.SetFloat(AnimatorSpeedHash, PlayerSpeedModifier);
        }
    }
    int AnimatorVelocityHash = 0, AnimatorSpeedHash = 0, MoveXHash, MoveZHash;
    public float AimSensitivity
    {
        get
        {
            return aimSensitivity;
        }
        set
        {
            aimSensitivity = value;
        }
    }

    public GameObject Player;
    public GameObject Gun;
    public CinemachineVirtualCamera AimCamera, thirdPersonCamera;
    CharacterController characterController;
    public Camera cam;
    public Transform modelToMove;
    public Transform CameraReference;
    public float aimSensitivity = 1f;
    Vector2 cameraRotationVec2FromMouse;
    Vector3 MovementVector;
    Vector2 direction;
    Controls controls;
    public Animator Anim;
    public Canvas PauseCanvas;
    Quaternion cameraQuatForMovement;
    

    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform point;
    [SerializeField] private Rig rigAim;

    Vector3 playerVel;

    bool isGrounded;
    public float jumpHeight = 5f;
    bool jumpPressed = false;
    float gravityValue = -9.81f;
    float JumpRayCastCd = 0f;
    float jumpCooldown = 0.1f;
    Vector2 accum = Vector2.zero;
    float aimRigWeight;
    bool isAiming = false;

    private void Awake()
    {
        controls = new Controls();
    }

    void OnEnable()
    {
        Anim.SetLayerWeight(1, 1);
        characterController = Player.GetComponent<CharacterController>();
        controls.Player.Enable();
        controls.Player.RotateCamera.performed += OnCameraRotate;
        AnimatorVelocityHash = Animator.StringToHash("Velocity");
        MoveXHash = Animator.StringToHash("MoveX");
        MoveZHash = Animator.StringToHash("MoveZ");
        AnimatorSpeedHash = Animator.StringToHash("SpeedMultiplier");

        //setting up the events for the input
        controls.Player.Movement.performed += cntxt => OnMovement(cntxt.ReadValue<Vector2>());
        controls.Player.Movement.canceled += OnMovementCanceled;
        controls.Player.Sprint.performed += ShiftPressed;
        controls.Player.Sprint.canceled += ShiftReleased;
        controls.Player.Jump.started += SpacePressed;
        controls.Player.Jump.canceled += SpaceReleased;
        //controls.Player.Gun.performed += GunPressed;
        //controls.Player.GunAway.performed += GunAwayPressed;
        //controls.Player.GunAway.canceled += GunAwayReleased;
        controls.Player.Shot.performed += ShotPressed;
        controls.Player.Shot.canceled += ShotReleased;
        controls.Player.Pause.performed += PauseGame;
    }
    void PauseGame(InputAction.CallbackContext ctxt)
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
            this.PlayerSpeed = 0;
            PauseCanvas.gameObject.SetActive(isGamePaused);
        }
        else
        {
            isGamePaused = false;

            PlayerSpeed = 1f;
            PauseCanvas.gameObject.SetActive(isGamePaused);

        }
    }
    void OnCameraRotate(InputAction.CallbackContext context)
    {
        if (isGamePaused)
            return;
        Vector2 lookValue = context.ReadValue<Vector2>();

        // lookValue.y = Mathf.Clamp(lookValue.y, -70f,70f);
        cameraRotationVec2FromMouse.x -= lookValue.y * AimSensitivity * Time.deltaTime;
        cameraRotationVec2FromMouse.y += lookValue.x * AimSensitivity * Time.deltaTime;
        cameraRotationVec2FromMouse.x = Mathf.Clamp(cameraRotationVec2FromMouse.x, -80f, 80f);
        CameraReference.transform.rotation = Quaternion.Euler(cameraRotationVec2FromMouse.x, cameraRotationVec2FromMouse.y, 0);

        //optimized quaternion fetching so i store it in memory only when i rotate the camera instead of every frame (moved here from update)
        Vector3 camForward = CameraReference.forward;
        //fetching the quaternion of the now rotated camera, to rotate the movement vector
        cameraQuatForMovement = Quaternion.LookRotation(
            new Vector3(camForward.x, 0, camForward.z),
            Vector3.up);
    }
    public void OnMovement(Vector2 dir)
    {
        //the direction i am going towards
        direction = dir;

        #region Useless but preserve
        //float strafe = movementVector.x;
        //float forward = movementVector.y;



        //if (!toMove.isGrounded)
        //{
        //    vertical = Physics.gravity.y * Time.deltaTime;

        //}
        //else
        //{
        //    vertical = 0;
        //}
        //forceToAdd += Camera.transform.forward * forward;
        //forceToAdd += Camera.transform.right * strafe;
        //forceToAdd.Normalize();
        //forceToAdd.y = vertical;

        //fixedForward = forceToAdd;
        //fixedForward.y = 0; 
        #endregion
    }
    public void OnMovementCanceled(InputAction.CallbackContext context)
    {
        MovementVector = Vector3.zero;
        direction = Vector2.zero;
    }
    // Update is called once per frame
    void Update()
    {
        if (isGamePaused)
            return;
        MoveRelativeToCameraRotation();
        rigAim.weight = Mathf.Lerp(rigAim.weight, aimRigWeight, Time.deltaTime * 20f);
    }
    void FixedUpdate()
    {
        if (isGamePaused)
            return;
        Aim();
        isGrounded = isGroundedTest();
        Anim.SetBool("isGrounded", isGrounded);
        GravityAndJumpUpdate();
    }
    void GravityAndJumpUpdate()
    {
        //groundedPlayer = characterController.isGrounded;
        if (isGrounded)
        {
            playerVel = Vector3.zero;
            jumpCooldown -= Time.deltaTime;
            jumpCooldown = Mathf.Clamp(jumpCooldown, 0f, 1f);
            //Anim.SetBool("isGrounded", true);
            if (!isAiming)
                Anim.applyRootMotion = true;
        }
        else
        { //i am jumping
            playerVel.x = MovementVector.x * 3f;
            playerVel.z = MovementVector.z * 3f;
        }

        if (jumpPressed && isGrounded)
        { // i am grounded and i want to jump
            JumpRayCastCd = 1f;
            jumpPressed = false;
            playerVel.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            Anim.SetBool("Jump", true);
            Anim.applyRootMotion = false;
            jumpCooldown = 0.3f;

        }

        playerVel.y += gravityValue * Time.deltaTime * 1.5f;
        characterController.Move(playerVel * Time.deltaTime);
    }
    bool isGroundedTest()
    {
        if (JumpRayCastCd > 0)
        {
            JumpRayCastCd -= Time.deltaTime;
            return false;
        }
        else
        {
            Ray groundedTest = new Ray(this.transform.position, Vector3.up * -0.5f);
            return Physics.Raycast(groundedTest, 0.1f);
        }
    }
    void ShiftPressed(InputAction.CallbackContext context)
    {
        //GetComponent<Animator>().SetBool("Shift", true);
        if (!isAiming)
            Anim.SetBool("Shift", true);
    }
    void ShiftReleased(InputAction.CallbackContext context)
    {
        Anim.SetBool("Shift", false);

        //GetComponent<Animator>().SetBool("Shift", false);
    }
    void SpacePressed(InputAction.CallbackContext context)
    {
        if (jumpCooldown <= 0 && !isAiming)
        {
            jumpPressed = true;
        }
    }
    void SpaceReleased(InputAction.CallbackContext context)
    {
        jumpPressed = false;
        GetComponent<Animator>().SetBool("Jump", false);

    }
    void GunPressed(InputAction.CallbackContext context)
    {
        Gun.SetActive(true);
        isAiming = true;
        thirdPersonCamera.Priority = 0;
        AimCamera.Priority = 30;
        Anim.applyRootMotion = false;
        aimRigWeight = 1f;
        GetComponent<Animator>().SetBool("isDrawingTheGun", isAiming);
    }
    void GunAwayPressed(InputAction.CallbackContext context)
    {
        Gun.SetActive(false);
        GetComponent<Animator>().SetBool("isGunPutAway", true);
        isAiming = false;
        thirdPersonCamera.Priority = 30;
        AimCamera.Priority = 0;
        Anim.applyRootMotion = true;
        aimRigWeight = 0f;
        GetComponent<Animator>().SetBool("isDrawingTheGun", false);

    }
    void GunAwayReleased(InputAction.CallbackContext context)
    {
        GetComponent<Animator>().SetBool("isGunPutAway", false);

    }
    void ShotPressed(InputAction.CallbackContext context)
    {
        GetComponent<Animator>().SetBool("Shot", true);
    }
    void ShotReleased(InputAction.CallbackContext context)
    {
        GetComponent<Animator>().SetBool("Shot", false);
    }
    void MoveRelativeToCameraRotation()
    {
        //Vector3 camForward = CameraReference.forward;
        ////fetching the quaternion of the now rotated camera, to rotate the movement vector
        //Quaternion q = Quaternion.LookRotation(
        //    new Vector3(camForward.x, 0, camForward.z),
        //    Vector3.up);

        //rotating the direction vector according to camera 
        Vector3 fromAbsoluteToRelative = cameraQuatForMovement * new Vector3(direction.x, 0, direction.y);

        // applying rot to vector, it is now relative to camera
        MovementVector = fromAbsoluteToRelative;

        float magnitude = MovementVector.magnitude;
        Anim.SetFloat(AnimatorVelocityHash, magnitude);

        Quaternion contextualQuaternion;
        if (magnitude > 0.05f)
        {
            contextualQuaternion = Quaternion.LookRotation(MovementVector, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, contextualQuaternion, 0.1f);
        }
        if (isAiming)
        {
            //root motion is now disabled
            characterController.Move(MovementVector * Time.deltaTime);
            transform.rotation = cameraQuatForMovement;
            Anim.SetFloat(AnimatorVelocityHash, magnitude);

            if (magnitude > 0.05f)
            {
                accum.y = Mathf.Lerp(accum.y, direction.y, 0.3f);
                accum.x = Mathf.Lerp(accum.x, direction.x, 0.3f);
            }
            else
            {
                accum.x = Mathf.Lerp(accum.x, 0, 0.3f);
                accum.y = Mathf.Lerp(accum.y, 0, 0.3f);
            }
            Anim.SetFloat(MoveZHash, accum.y);
            Anim.SetFloat(MoveXHash, accum.x);
        }


    }
    void Aim()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = cam.ScreenPointToRay(screenCenterPoint);
        //Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask))
        {
            point.position = hit.point;
            mouseWorldPosition = hit.point;
        }
        if (isAiming)
        {
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }

    }
}


