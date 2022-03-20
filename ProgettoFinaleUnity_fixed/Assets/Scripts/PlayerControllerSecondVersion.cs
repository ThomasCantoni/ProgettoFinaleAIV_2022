using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerControllerSecondVersion : MonoBehaviour
{
    public GameObject Gun;
    public CinemachineVirtualCamera AimCamera, ThirdPersonCamera;
    
   
    public CharacterController characterController;
    Vector2 cameraRotationVec2FromMouse;
    Vector3 MovementVector;
    Vector2 dir;
    InputAction move;
    Controls controls;
    int AnimatorVelocityHash = 0;
    public Animator Anim;
    public GameObject Player;
    public Transform modelToMove;
    public Transform CameraReference;
    public float AimSensitivity = 1f;
    public float Speed = 2.5f;
    public float jumpHeight = 5f;
    
    
    Vector3 playerVel;
    bool isGrounded;
    bool jumpPressed = false;
    float gravityValue = -9.81f;
    float JumpRayCastCd = 0f;
    float jumpCooldown = 0.1f;
   
    bool isAiming = false;


    float vertical = 0;
   
    private void Awake()
    {
        controls = new Controls();
    }
    void OnEnable()
    {
        
            Anim.SetLayerWeight(1, 1);
            characterController = Player.GetComponent<CharacterController>();
            controls.Player.Enable();
            controls.Player.Aim.performed += OnCameraRotate;
            AnimatorVelocityHash = Animator.StringToHash("Velocity");
            //AnimatorVelocityHash = Animator.StringToHash("MoveX");
            //AnimatorVelocityHash = Animator.StringToHash("MoveZ");


            //setting up the events for the input
            controls.Player.Movement.performed += cntxt => OnMovement(cntxt.ReadValue<Vector2>());
            controls.Player.Movement.canceled += OnMovementCanceled;
            controls.Player.Sprint.performed += ShiftPressed;
            controls.Player.Sprint.canceled += ShiftReleased;
            controls.Player.Jump.started += SpacePressed;
            controls.Player.Jump.canceled += SpaceReleased;
            controls.Player.Gun.performed += GunPressed;
            controls.Player.GunAway.performed += GunAwayPressed;
            controls.Player.GunAway.canceled += GunAwayReleased;
            controls.Player.Shot.performed += ShotPressed;
            controls.Player.Shot.canceled += ShotReleased;

        
    }
    void OnCameraRotate(InputAction.CallbackContext context)
    {
        Vector2 lookValue = context.ReadValue<Vector2>();
        
       // lookValue.y = Mathf.Clamp(lookValue.y, -70f,70f);
        cameraRotationVec2FromMouse.x -= lookValue.y * AimSensitivity * Time.deltaTime;
        cameraRotationVec2FromMouse.y += lookValue.x * AimSensitivity * Time.deltaTime;
        cameraRotationVec2FromMouse.x = Mathf.Clamp(cameraRotationVec2FromMouse.x, -80f,80f) ;
        
    }
    public void OnMovement(Vector2 direction)
    {
        //the direction i am going towards
        dir = direction;
        

        Vector3 camForward = CameraReference.forward;
        //fetching the quaternion of the now rotated camera, to rotate the movement vector
        Quaternion q = Quaternion.LookRotation(
            new Vector3(camForward.x, 0, camForward.z),
            Vector3.up);

        //rotating the direction vector according to camera 
        Vector3 cooking = q * new Vector3(dir.x, 0, dir.y);

        // applying rot to vector
        MovementVector = cooking;


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
        dir = Vector2.zero;
    }
    // Update is called once per frame
    void Update()
    {
        
        AdjustCamera();
        MoveRelativeToCameraRotation();
       
    }
    void FixedUpdate()
    {
       isGrounded = IsGroundedTest();
        Anim.SetBool("isGrounded", isGrounded);
        GravityAndJumpUpdate();
       // ApplyGravity();
    }
    public bool IsGroundedTest()
    {
        if (JumpRayCastCd > 0)
        {
            JumpRayCastCd = JumpRayCastCd - Time.deltaTime;
            return false;
        }
        else
        {
            Ray groundedTest = new Ray(this.transform.position, Vector3.up * -0.5f);
            return Physics.Raycast(groundedTest, 0.1f);

        }
        

    }
    void ApplyGravity()
    {
        if (isGrounded)
        {
            vertical = 0;
        }

        else
        {
            vertical += Physics.gravity.y * Time.deltaTime;
        
        }
        //applying the gravity
       
        characterController.Move(new Vector3(0, vertical, 0));
    }
    void MoveRelativeToCameraRotation()
    {
        //Vector3 camForward = CameraReference.forward;
        ////fetching the quaternion of the now rotated camera, to rotate the movement vector
        //Quaternion q = Quaternion.LookRotation(
        //    new Vector3(camForward.x, 0, camForward.z),
        //    Vector3.up);

        ////rotating the direction vector according to camera 
        //Vector3 cooking = q * new Vector3(dir.x, 0, dir.y);

        //// applying rot to vector
        //MovementVector = cooking;

        float magnitude = MovementVector.magnitude;
          Anim.SetFloat(AnimatorVelocityHash, magnitude);

        Quaternion contextualQuaternion;
        if (magnitude > 0.05f )
        {
          contextualQuaternion = Quaternion.LookRotation(MovementVector, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation,contextualQuaternion,0.1f);


        }
        if (isAiming)
        {
            //if the 
            Vector3 cameraForward = CameraReference.forward;
            //root motion is now disabled
            characterController.Move(MovementVector*Time.deltaTime);
          
            cameraForward.y = 0;
            contextualQuaternion = Quaternion.LookRotation(cameraForward,Vector3.up);
            transform.rotation = contextualQuaternion;

        }


    }
    
    void ShiftPressed(InputAction.CallbackContext context)
    {
        if(!isAiming)
            Anim.SetBool("Shift", true);
    }
    void ShiftReleased(InputAction.CallbackContext context)
    {
        Anim.SetBool("Shift", false);
    }
    void AdjustCamera()
    {
        //i need this to rotate the motion vector according to the direction of the camera


        //rotating the player camera according to the mouse input
        CameraReference.transform.rotation = Quaternion.Euler(cameraRotationVec2FromMouse.x, cameraRotationVec2FromMouse.y, 0);
        return;
        //fetching the quaterion of the now rotated camera, to rotate the movement vector
        //Quaternion q = Quaternion.LookRotation(
        //    new Vector3(CameraReference.transform.forward.x, 0, CameraReference.transform.forward.z),
        //    Vector3.up);

        ////rotating the direction vector according to camera 
        //Vector3 cooking = q * new Vector3(dir.x, 0, dir.y);

        //// applying rot to vector
        //MovementVector = cooking;
    }
    //void OnJump()
    //{
    //    if (characterController.velocity.y == 0)
    //    {
    //        jumpPressed = true;
    //    }
    //    else
    //    {

    //    }
    //}
    void GravityAndJumpUpdate()
    {
        //groundedPlayer = characterController.isGrounded;
        if (isGrounded)
        {
            playerVel.y = 0.0f;
            jumpCooldown -= Time.deltaTime;
            jumpCooldown = Mathf.Clamp(jumpCooldown, 0f, 1f);
            //Anim.SetBool("isGrounded", true);
            if(!isAiming)
            Anim.applyRootMotion = true;
        }
        else
        {
            playerVel.x = MovementVector.x*3f;
            playerVel.z = MovementVector.z*3f;
        }
        
       if (jumpPressed && isGrounded)
        {
            JumpRayCastCd = 1f;
            jumpPressed = false;
            playerVel.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            Anim.SetBool("Jump", true);
            Anim.applyRootMotion = false;
            jumpCooldown = 0.3f;
            
        }
        
        playerVel.y += gravityValue * Time.deltaTime*1.5f;
        characterController.Move(playerVel * Time.deltaTime);
    }
    void SpacePressed(InputAction.CallbackContext context)
    {
        if(jumpCooldown <=0)
        jumpPressed = true;
   
        
        //MovementJump();
        //GetComponent<Animator>().SetBool("isGrounded", toMove.isGrounded);
    }
    void SpaceReleased(InputAction.CallbackContext context)
    {
        jumpPressed = false;
        GetComponent<Animator>().SetBool("Jump", false);
    }

    void GunPressed(InputAction.CallbackContext context)
    {
        Gun.SetActive(true);
        ThirdPersonCamera.Priority = 0;
        AimCamera.Priority = 30;
        Anim.applyRootMotion = false;
        isAiming = true;
        GetComponent<Animator>().SetBool("isDrawingTheGun", isAiming);
    }
    void GunAwayPressed(InputAction.CallbackContext context)
    {
        Gun.SetActive(false);
        GetComponent<Animator>().SetBool("isGunPutAway", true);
        isAiming = false;
        ThirdPersonCamera.Priority = 30;
        AimCamera.Priority = 0;
        Anim.applyRootMotion = true;

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
}
