using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PlayerControllerSecondVersion : MonoBehaviour
{
    public bool GunEquipped
    {
        get
        {
            return this.GetComponent<InverseKinematicsTest>().GunEquipped;
        }
    }

    public float PlayerSpeed
    {
        get
        {
            return Time.unscaledDeltaTime * TimeManager.PlayerCurrentSpeed;
        }
    }

    public float FOV
    {
        get
        {
            return ThirdPersonCamera.m_Lens.FieldOfView;
        }
        set
        {
            ThirdPersonCamera.m_Lens.FieldOfView = value;
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
            return Physics.gravity.y * Time.deltaTime;
        }
    }

    public bool IsGamePaused = false;
    public AudioSource BulletTimeAudioSource;
    public AudioMixerGroup Mixer;
    public CinemachineVirtualCamera AimCamera, ThirdPersonCamera;
    public Camera Camera;
    public CharacterController characterController;
    public GameObject Player;
    public Transform CameraReference;
    public Canvas PauseCanvas;
    public LayerMask SphereCastLayers;
    public GroundedCollider GroundedCollider;
    public bool UseLatestData = false;
    public PlayerData PlayerData;
    [HideInInspector]
    public EllenActionPoints EllenAp;
    [HideInInspector]
    public Animator Anim;
    [HideInInspector]
    public Controls Controls;
    [HideInInspector]
    public bool IsAiming = false;

    private float aimSensitivity;
    private int animatorVelocityHash = 0, animatorSpeedHash = 0;
    private float jumpHeight = 5f;
    private float speedInAir = 2.5f;
    private float speedWhileAiming = 2f;
    private Vector3 playerVel;
    private Vector2 cameraRotationVec2FromMouse;
    private Vector3 movementVector;
    private Vector2 direction;
    private Quaternion cameraQuatForMovement;
    private bool isGrounded;
    private bool jumpPressed = false;
    private float gravityValue = -9.81f;
    private float groundedCheckCooldown = 0.1f;
    private float jumpCooldown = 0.1f;
    private Vector2 accum = Vector2.zero;
    private float speedAirMulti = 1f;
    private float speedInAirMult = 1.8f;

    void OnEnable()
    {
        Controls = new Controls();
        Anim = GetComponent<Animator>();
        EllenAp = GetComponent<EllenActionPoints>();
        LoadData();
        SetPrefs();
    }

    void LoadData()
    {
        if(!UseLatestData)
        {
            return;
        }
        for(int i=0;i<2;i++)
        {
            if (SaveManager.LastSave == null)
            {
                SaveManager.LoadPlayer(Application.persistentDataPath + "/playerData.dat");
                if(i==1)
                {
                    return;
                }
            }
            else
            {
                break;
            }
        }
        this.PlayerData = SaveManager.LastSave;
        if (PlayerData.IsNewGame)
        {
            PlayerPrefs.SetFloat(SaveManager.FOV,50f);
            PlayerPrefs.SetFloat(SaveManager.AimSensitivity,5f);

            //overwrite data with current data
            PlayerData = new PlayerData(this);
            SaveManager.SavePlayer(PlayerData);
            return;
        }
        characterController.enabled = false;
        this.transform.position = new Vector3(PlayerData.playerPosX, PlayerData.playerPosY, PlayerData.playerPosZ);
        characterController.enabled = true;
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
        Cursor.visible = false;
        Anim.SetLayerWeight(1, 1);
        
        characterController = Player.GetComponent<CharacterController>();
        animatorVelocityHash = Animator.StringToHash("Velocity");
        animatorSpeedHash = Animator.StringToHash("SpeedMultiplier");

        //setting up the events for the input
        Controls.Player.Enable();
        Controls.Player.RotateCamera.performed += OnCameraRotate;
        Controls.Player.Zoom.performed += OnZoom;
        Controls.Player.Zoom.canceled += OnZoomCancel;
        Controls.Player.Movement.performed += cntxt => OnMovement(cntxt.ReadValue<Vector2>());
        Controls.Player.Movement.canceled += OnMovementCanceled;
        Controls.Player.Sprint.performed += ShiftPressed;
        Controls.Player.Sprint.canceled += ShiftReleased;
        Controls.Player.Jump.started += SpacePressed;
        Controls.Player.Jump.canceled += SpaceReleased;
        Controls.Player.Pause.performed += PauseGame;
        Controls.Player.BulletTimeInput.performed += ManageBulletTimePlayerSide;
    }

    public void PlayerActivateBT(InputAction.CallbackContext ctxt)
    {
        AimSensitivity = 5f;
    }
    
    void PauseGame(InputAction.CallbackContext ctxt)
    {
        if (Anim == null)
        {
            Anim = GetComponent<Animator>();
        }

        if (TimeManager.IsGamePaused == false)
        {
            TimeManager.EnablePause();
            Anim.SetFloat(animatorSpeedHash, TimeManager.PlayerCurrentSpeed);

            PauseCanvas.gameObject.SetActive(true);
        }
        else
        {
            TimeManager.DisablePause();
            Anim.SetFloat(animatorSpeedHash, TimeManager.PlayerCurrentSpeed);

            PauseCanvas.gameObject.SetActive(false);
        }

    }

    public void Respwan()
    {
        LoadData();
        Controls.Enable();
        Destroy(GetComponent<CameraOut>());
        SetPrefs();
       
    }

    void OnZoom(InputAction.CallbackContext context)
    {
        if (TimeManager.IsGamePaused || !GunEquipped)
        {
            return;
        }
        IsAiming = true;
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
        IsAiming = false;
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
        cameraRotationVec2FromMouse.x = Mathf.Clamp(cameraRotationVec2FromMouse.x, -50f, 70f);
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
        movementVector = Vector3.zero;
        direction = Vector2.zero;
    }

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
            Anim.SetFloat(animatorSpeedHash, TimeManager.PlayerCurrentSpeed);
            Mixer.audioMixer.SetFloat("Pitch", 1f);
            BulletTimeAudioSource.Stop();
        }
        if(BulletTimeAudioSource.isPlaying)
        {
            BulletTimeAudioSource.pitch = 1.5f - (GetComponent<EllenActionPoints>().AP_Value / GetComponent<EllenActionPoints>().MaxAp)*0.8f;
        }
        MoveRelativeToCameraRotation();
        GravityAndJumpUpdate();
    }

    void GravityAndJumpUpdate()
    {
        if (groundedCheckCooldown > 0f)
        {
            groundedCheckCooldown -= Time.deltaTime;
            GroundedCollider.Disable();
        }
        else
        {
            GroundedCollider.Enable();
            isGrounded = GroundedCollider.touching;
            Anim.SetBool("isGrounded", isGrounded);
            GlobalVariables.IsPlayerGrounded = isGrounded;
        }

        if (isGrounded)
        {
            playerVel = Vector3.zero;
            playerVel.y = -1f;
            jumpCooldown -= Time.deltaTime;
            jumpCooldown = Mathf.Clamp(jumpCooldown, 0f, 1f);
            GroundedCollider.SwitchToBig();
            if (!IsAiming)
                Anim.applyRootMotion = true;
        }
        else
        { //i am in the air
            GroundedCollider.SwitchToSmall();
            playerVel.x = movementVector.x * speedInAir * speedAirMulti;
            playerVel.z = movementVector.z * speedInAir * speedAirMulti;
        }

        if (jumpPressed && isGrounded)
        { // i am grounded and i want to jump
            isGrounded = false;
            GlobalVariables.IsPlayerGrounded = isGrounded;
            GroundedCollider.Disable();
            groundedCheckCooldown = 0.2f;
            jumpPressed = false;
            playerVel.y = 0f;
            playerVel.y += Mathf.Sqrt(jumpHeight * gravityValue * -1f);
            Anim.SetBool("Jump", true);
            Anim.applyRootMotion = false;
            jumpCooldown = 0.3f;
        }
        playerVel.y += GravityScaled;
        GlobalVariables.PlayerVelocityNotGrounded = playerVel;
        characterController.Move(playerVel * PlayerSpeed);
    }

    public bool IsGroundedTest()
    {
        if (groundedCheckCooldown > 0)
        {
            groundedCheckCooldown = groundedCheckCooldown - Time.deltaTime;
            return false;
        }
        else
        {
            Ray groundedTest = new Ray(this.transform.position + Vector3.up * 0.7f, Vector3.up * -0.5f);
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
            BulletTimeAudioSource.Play();
            Mixer.audioMixer.SetFloat("Pitch", 0.3f);
        }
        else
        {
            EllenAp.Disable();
            Mixer.audioMixer.SetFloat("Pitch", 1f);
            BulletTimeAudioSource.Stop();
        }

        Anim.SetFloat(animatorSpeedHash, TimeManager.PlayerCurrentSpeed);
    }

    void MoveRelativeToCameraRotation()
    {
        Vector3 fromAbsoluteToRelative = cameraQuatForMovement * new Vector3(direction.x, 0, direction.y);
        movementVector = fromAbsoluteToRelative;
        GlobalVariables.PlayerVelocityGrounded = movementVector;
        float magnitude = movementVector.magnitude;
        Anim.SetFloat(animatorVelocityHash, magnitude);

        Quaternion contextualQuaternion;
        if (magnitude > 0.05f)
        {
            contextualQuaternion = Quaternion.LookRotation(movementVector, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, contextualQuaternion, 0.1f);
        }
        if (IsAiming)
        {

            //root motion is now disabled
            characterController.Move(movementVector * PlayerSpeed * speedWhileAiming);

            transform.rotation = cameraQuatForMovement;
            if (magnitude > 0.05f)
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
        if (!IsAiming)
        {
            Anim.SetBool("Shift", true);
            speedAirMulti = speedInAirMult;
        }
    }

    void ShiftReleased(InputAction.CallbackContext context)
    {
        Anim.SetBool("Shift", false);
        speedAirMulti = 1f;
    }

    void SpacePressed(InputAction.CallbackContext context)
    {
        if (jumpCooldown <= 0 && isGrounded)
            jumpPressed = true;
    }
    void SpaceReleased(InputAction.CallbackContext context)
    {
        jumpPressed = false;
        GetComponent<Animator>().SetBool("Jump", false);
    }

    private void OnDisable()
    {
        Controls.Disable();
    }
}
