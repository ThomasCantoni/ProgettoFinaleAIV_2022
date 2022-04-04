using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class PlayerControllerSecondVersion : MonoBehaviour
{

    public PlayerData PlayerData;
    public bool GunEquipped
    {
        get
        {
            return this.GetComponent<InverseKinematicsTest>().GunEquipped;
        }

    }
    public bool isGamePaused = false;
    public float PlayerSpeed 
    {
        get
        {
            
            return Time.unscaledDeltaTime * TimeManager.PlayerCurrentSpeed;
        }
        
    }
    int AnimatorVelocityHash = 0,AnimatorSpeedHash=0;
    public float FOV 
    { get
        {
            return ThirdPersonCamera.m_Lens.FieldOfView;
        }
        set
        {
            ThirdPersonCamera.m_Lens.FieldOfView= value;
            AimCamera.m_Lens.FieldOfView = value;


        }
    }
    public float AimSensitivity
    {
        get
        {
            return aimSensitivity;
        }
        set
        {
            aimSensitivity = value;
            PauseCanvas.transform.GetComponentInChildren<Slider>().value = value;
        }
    }
    public float GravityScaled 
    {
        get
        {
            return gravityValue * PlayerSpeed;
        }
        
    }

    public EllenActionPoints EllenAp;
    public CinemachineVirtualCamera AimCamera, ThirdPersonCamera;
    public Camera Camera;
    public CharacterController characterController;
    public Animator Anim;
    public GameObject Player;
   // public Transform modelToMove;
    public Transform CameraReference;
    private float aimSensitivity = 1f;
    //public float Speed = 2.5f;
    public float jumpHeight = 5f;
    public Canvas PauseCanvas;
    public LayerMask SphereCastLayers;
    Vector3 playerVel;
    Vector2 cameraRotationVec2FromMouse;
    Vector3 MovementVector;
    Vector2 direction;
    Quaternion cameraQuatForMovement;
    public Controls controls;
    bool isGrounded;
    bool jumpPressed = false;
    public bool isAiming = false;
   
    float gravityValue = -9.81f;
    float JumpRayCastCd = 0f;
    float jumpCooldown = 0.1f;
    Vector2 accum = Vector2.zero;
    



    private void GetStuff()
    {
       
    }
    private void OnEnable()
    {
        
        controls = new Controls();
        Anim = GetComponent<Animator>();
        EllenAp = GetComponent<EllenActionPoints>();
        SetPrefs();
        LoadData();
    }
    void LoadData()
    {
        if(SaveManager.LastSave == null)
        {
            Debug.LogError("COULD NOT LOAD SAVEFILE");
            return;
        }
        this.PlayerData = SaveManager.LastSave;
        if(PlayerData.IsNewGame )
        {
            return;
        }
        this.transform.position = new Vector3(PlayerData.playerPosX, PlayerData.playerPosY,PlayerData.playerPosZ) ;
        this.GetComponent<EllenHealthScript>().HP_Value = PlayerData.PlayerHp;
        EllenAp.AP_Value = PlayerData.PlayerAp;
    }
    void SetPrefs()
    {
        AimSensitivity = PlayerPrefs.GetFloat(SaveManager.AimSensitivity);
        FOV = PlayerPrefs.GetFloat(SaveManager.FOV);
    }
    void Start()
    {
        
           Anim.SetLayerWeight(1, 1);
            characterController = Player.GetComponent<CharacterController>();
            AnimatorVelocityHash = Animator.StringToHash("Velocity");
            AnimatorSpeedHash = Animator.StringToHash("SpeedMultiplier");
            //AnimatorVelocityHash = Animator.StringToHash("MoveX");
            //AnimatorVelocityHash = Animator.StringToHash("MoveZ");


            //setting up the events for the input
            controls.Player.Enable();
            controls.Player.RotateCamera.performed += OnCameraRotate;
            controls.Player.Zoom.performed += OnZoom;
            controls.Player.Zoom.canceled += OnZoomCancel;
            controls.Player.Movement.performed += cntxt => OnMovement(cntxt.ReadValue<Vector2>());
            controls.Player.Movement.canceled += OnMovementCanceled;
            controls.Player.Sprint.performed += ShiftPressed;
            controls.Player.Sprint.canceled += ShiftReleased;
            controls.Player.Jump.started += SpacePressed;
            controls.Player.Jump.canceled += SpaceReleased;
            controls.Player.Pause.performed+= PauseGame;
            //controls.Player.BulletTimeInput.performed += TimeManager.EnableBulletTime;
            controls.Player.BulletTimeInput.performed += ManageBulletTimePlayerSide;
            



        
    }
    public void PlayerActivateBT(InputAction.CallbackContext ctxt)
    {

    }
    void PauseGame(InputAction.CallbackContext ctxt)
    {
            if(Anim == null)
            {
                Anim = GetComponent<Animator>();
            }
        
        if(TimeManager.IsGamePaused == false)
        {
            TimeManager.EnablePause();
            Debug.Log(Anim);
            Anim.SetFloat(AnimatorSpeedHash, TimeManager.PlayerCurrentSpeed);

            PauseCanvas.gameObject.SetActive(true);
        }
        else
        {
            TimeManager.DisablePause();
            Debug.Log(Anim);
            Anim.SetFloat(AnimatorSpeedHash,TimeManager.PlayerCurrentSpeed);

            PauseCanvas.gameObject.SetActive(false);
        }

    }
    void OnZoom(InputAction.CallbackContext context)
    {
        if(TimeManager.IsGamePaused || !GunEquipped)
        {
            return;
        }
        isAiming = true;
        Anim.SetBool("IsAiming", true);
        ThirdPersonCamera.Priority = 0;
        AimCamera.Priority = 30;
        Anim.applyRootMotion = false;
        
    }
    void OnZoomCancel(InputAction.CallbackContext context)
    {
        if (TimeManager.IsGamePaused)
        {
            return;
        }
        isAiming = false;
        Anim.SetBool("IsAiming", false);

        ThirdPersonCamera.Priority = 30;
        AimCamera.Priority = 0;
        
       
        Anim.applyRootMotion = true;
        
    }
    void OnCameraRotate(InputAction.CallbackContext context)
    {
        if (TimeManager.IsGamePaused)
            return;
        Vector2 lookValue = context.ReadValue<Vector2>();
        
       // lookValue.y = Mathf.Clamp(lookValue.y, -70f,70f);
        cameraRotationVec2FromMouse.x -= lookValue.y * AimSensitivity * Time.deltaTime;
        cameraRotationVec2FromMouse.y += lookValue.x * AimSensitivity * Time.deltaTime;
        cameraRotationVec2FromMouse.x = Mathf.Clamp(cameraRotationVec2FromMouse.x, -50f,70f) ;
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


        //Vector3 camForward = CameraReference.forward;
        ////fetching the quaternion of the now rotated camera, to rotate the movement vector
        //Quaternion q = Quaternion.LookRotation(
        //    new Vector3(camForward.x, 0, camForward.z),
        //    Vector3.up);

        ////rotating the direction vector according to camera 
        //Vector3 cooking = q * new Vector3(dir.x, 0, dir.y);

        //// applying rot to vector
        //MovementVector = cooking;
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
        if (TimeManager.IsGamePaused)
        {
            return;
        }
        if (EllenAp.AP_Value <= 0f && EllenAp.isActive)
        {
            TimeManager.DisableBulletTime();
            EllenAp.Disable();
            Anim.SetFloat(AnimatorSpeedHash, TimeManager.PlayerCurrentSpeed);
        }

        MoveRelativeToCameraRotation();
        isGrounded = IsGroundedTest();
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
            if(!isAiming)
            Anim.applyRootMotion = true;
        }
        else
        { //i am jumping
            playerVel.x = MovementVector.x*2.5f;
            playerVel.z = MovementVector.z*2.5f;
        }

        if (jumpPressed && isGrounded)
        { // i am grounded and i want to jump
            JumpRayCastCd = 0.2f;
            jumpPressed = false;
            playerVel.y += Mathf.Sqrt(jumpHeight  * gravityValue * -1f);
            Anim.SetBool("Jump", true);
            Anim.applyRootMotion = false;
            jumpCooldown = 0.3f;

        }

        playerVel.y += GravityScaled;
        characterController.Move(playerVel * PlayerSpeed);
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
            Ray groundedTest = new Ray(this.transform.position+Vector3.up*0.7f, Vector3.up * -0.5f);
            Debug.DrawRay(this.transform.position + Vector3.up ,Vector3.down,Color.red,1f);
            return Physics.SphereCast(groundedTest, 0.7f, 0.1f, SphereCastLayers);
            

        }
        

    }
    public void ManageBulletTimePlayerSide(InputAction.CallbackContext ctx)
    {
        if (EllenAp.Cooldown > 0f)
        {
            return;
        }
        TimeManager.EnableBulletTime();
        if (TimeManager.IsBulletTimeActive)
        {
            EllenAp.Activate();

        }
        else
        {
            EllenAp.Disable();
        }
        Anim.SetFloat(AnimatorSpeedHash, TimeManager.PlayerCurrentSpeed);
    }
    void MoveRelativeToCameraRotation()
    {
        //rotating the direction vector according to camera 
        Vector3 fromAbsoluteToRelative = cameraQuatForMovement * new Vector3(direction.x, 0, direction.y);

        // applying rot to vector, it is now relative to camera
        MovementVector = fromAbsoluteToRelative;

        float magnitude = MovementVector.magnitude;
          Anim.SetFloat(AnimatorVelocityHash, magnitude);

        Quaternion contextualQuaternion;
        if (magnitude > 0.05f )
        {
            contextualQuaternion = Quaternion.LookRotation(MovementVector, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation,contextualQuaternion,0.1f);
        }
        if (isAiming)
        //if the player is aiming
        {
            //root motion is now disabled
            characterController.Move(MovementVector*PlayerSpeed);
            transform.rotation = cameraQuatForMovement;
            if(magnitude > 0.05f)
            {
                accum.y = Mathf.Lerp(accum.y, direction.y, 0.3f);
                accum.x = Mathf.Lerp(accum.x, direction.x, 0.3f);

            }
            else
            {
                accum.y = Mathf.Lerp(accum.y, 0f, 0.3f);
                accum.x = Mathf.Lerp(accum.x, 0f, 0.3f);
            }
            Anim.SetFloat("MoveZ", accum.y);
            Anim.SetFloat("MoveX", accum.x);
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
    
    
    void SpacePressed(InputAction.CallbackContext context)
    {
        if(jumpCooldown <=0 && isGrounded)
        jumpPressed = true;
    }
    void SpaceReleased(InputAction.CallbackContext context)
    {
        jumpPressed = false;
        GetComponent<Animator>().SetBool("Jump", false);
    }

    
}
