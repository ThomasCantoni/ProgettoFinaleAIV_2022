using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class PlayerControllerSecondVersion : MonoBehaviour
{
    public bool isGamePaused = false;
    public float PlayerSpeed 
    {
        get
        {
            
            return Time.unscaledDeltaTime * TimeManager.PlayerCurrentSpeed;
        }
        
    }
    int AnimatorVelocityHash = 0,AnimatorSpeedHash=0;
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
    public CinemachineVirtualCamera AimCamera, ThirdPersonCamera;
    public Camera Camera;
    public CharacterController characterController;
    public Animator Anim;
    public GameObject Player;
    public Transform CameraReference;
    private float aimSensitivity = 1f;
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
    
    private void Awake()
    {
        controls = new Controls();
    }

    void OnEnable()
    {
            Anim.SetLayerWeight(1, 1);
            characterController = Player.GetComponent<CharacterController>();
            AnimatorVelocityHash = Animator.StringToHash("Velocity");
            AnimatorSpeedHash = Animator.StringToHash("SpeedMultiplier");

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
            controls.Player.BulletTimeInput.performed += TimeManager.EnableBulletTime;
            controls.Player.BulletTimeInput.performed += ManageBulletTimePlayerSide;;

            AimSensitivity = 5f;
    }
    
    void PauseGame(InputAction.CallbackContext ctxt)
    {
        
        if(TimeManager.IsGamePaused == false)
        {
            TimeManager.EnablePause();
            Anim.SetFloat(AnimatorSpeedHash, TimeManager.PlayerCurrentSpeed);

            PauseCanvas.gameObject.SetActive(true);
        }
        else
        {
            TimeManager.DisablePause();
            Anim.SetFloat(AnimatorSpeedHash,TimeManager.PlayerCurrentSpeed);

            PauseCanvas.gameObject.SetActive(false);
        }
    }
    void OnZoom(InputAction.CallbackContext context)
    {
        if(TimeManager.IsGamePaused)
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
        cameraRotationVec2FromMouse.x -= lookValue.y * AimSensitivity * Time.deltaTime;
        cameraRotationVec2FromMouse.y += lookValue.x * AimSensitivity * Time.deltaTime;
        cameraRotationVec2FromMouse.x = Mathf.Clamp(cameraRotationVec2FromMouse.x, -50f,80f) ;
        CameraReference.transform.rotation = Quaternion.Euler(cameraRotationVec2FromMouse.x, cameraRotationVec2FromMouse.y, 0);
        Vector3 camForward = CameraReference.forward;
        cameraQuatForMovement = Quaternion.LookRotation(
            new Vector3(camForward.x, 0, camForward.z),
            Vector3.up);
    }
    
    public void OnMovement(Vector2 dir)
    {
        direction = dir;
    }
    public void OnMovementCanceled(InputAction.CallbackContext context)
    {
        MovementVector = Vector3.zero;
        direction = Vector2.zero;
    }
    void Update()
    {
        if (TimeManager.IsGamePaused)
        {

            return;
        }
        MoveRelativeToCameraRotation();
        isGrounded = IsGroundedTest();
        Anim.SetBool("isGrounded", isGrounded);
        GravityAndJumpUpdate();
    }
    void GravityAndJumpUpdate()
    {
        if (isGrounded)
        {
            playerVel = Vector3.zero;
            jumpCooldown -= Time.deltaTime;
            jumpCooldown = Mathf.Clamp(jumpCooldown, 0f, 1f);
            if(!isAiming)
            Anim.applyRootMotion = true;
        }
        else
        { 
            playerVel.x = MovementVector.x*2.5f;
            playerVel.z = MovementVector.z*2.5f;
        }

        if (jumpPressed && isGrounded)
        { 
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
            Ray groundedTest = new Ray(this.transform.position+Vector3.up * 0.3f, Vector3.up * -0.5f);
            return Physics.SphereCast(groundedTest, 0.3f, 0.5f, SphereCastLayers);
        }
    }
    public void ManageBulletTimePlayerSide(InputAction.CallbackContext ctx)
    {
        Anim.SetFloat(AnimatorSpeedHash, TimeManager.PlayerCurrentSpeed);
    }
    void MoveRelativeToCameraRotation()
    {
        Vector3 fromAbsoluteToRelative = cameraQuatForMovement * new Vector3(direction.x, 0, direction.y);
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
        {
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
        if(jumpCooldown <=0)
        jumpPressed = true;
    }
    void SpaceReleased(InputAction.CallbackContext context)
    {
        jumpPressed = false;
        GetComponent<Animator>().SetBool("Jump", false);
    }
}
