using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System;

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
    int AnimatorVelocityHash = 0, AnimatorSpeedHash = 0;
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
    public bool UseLatestData = false;
    public PlayerData PlayerData;
    public bool isGamePaused = false;
    public AudioSource BulletTimeAudioSource, AmbientAudioSource;
    public AudioClip Lvl1_Ambient, Lvl2_ambient;
    public AudioMixerGroup Mixer;
    public EllenActionPoints EllenAp;
    public CinemachineVirtualCamera AimCamera, ThirdPersonCamera;
    public Camera Camera;
    public CharacterController characterController;
    public Animator Anim;
    public GameObject Player;



    public Transform CameraReference;
    private float aimSensitivity = 1f;
    public float jumpHeight = 5f;
    public float SpeedInAir = 2.5f;
    public float SpeedWhileAiming = 2f;
    public Canvas PauseCanvas;
    public Canvas KeyCanvas;
    public LayerMask SphereCastLayers;
    Vector3 playerVel;
    Vector2 cameraRotationVec2FromMouse;
    Vector3 MovementVector;
    Vector2 direction;
    Quaternion cameraQuatForMovement;
    public Controls Controls;
    public bool isGrounded;
    bool jumpPressed = false;
    public bool isAiming = false;



    float gravityValue = -9.81f;
    float GroundCheckCooldown = 0.1f;
    float jumpCooldown = 0.1f;
    Vector2 accum = Vector2.zero;
    float speedAirMulti = 1f;
    public float SpeedInAirMultiplier = 1.8f;

    public GroundedCollider GroundedCollider;

  
    private void OnEnable()
    {
        Controls = new Controls();
        Anim = GetComponent<Animator>();
        EllenAp = GetComponent<EllenActionPoints>();
        LoadData();
        SetPrefs();
        SelectAmbient();
    }

    private void SelectAmbient()
    {
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            AmbientAudioSource.clip = Lvl2_ambient;

        }
        else
        {
            AmbientAudioSource.clip = Lvl1_Ambient;

        }
        AmbientAudioSource.Play();
    }

    void LoadData()
    {
        if (!UseLatestData)
        {
            Debug.Log("Data not loaded. Uncheck the boolean 'UseLatestData' in PCSV \n if you wish to load the latest savefile");
            return;
        }
        //check save 
        for (int i = 0; i < 2; i++)
        {
            if (SaveManager.LastSave == null)
            {

                SaveManager.LoadPlayer(Application.persistentDataPath + "/playerData.dat");
                if (i == 1)
                {
                    Debug.LogError("SAVEFILE LOAD ATTEMPT FAILED TWICE");
                    return;
                }
            }
            else
            {
                Debug.Log("Savefile succesfully loaded");
                break;
            }
        }
        //check if save is new
        this.PlayerData = SaveManager.LastSave;
        if (PlayerData.IsNewGame)
        {
            PlayerPrefs.SetFloat(SaveManager.FOV, 50f);
            PlayerPrefs.SetFloat(SaveManager.AimSensitivity, 5f);

            //overwrite data with current data
            PlayerData = new PlayerData(this);
            SaveManager.SavePlayer(PlayerData);
            return;
        }
        //if save is not new, check scene
        else if (PlayerData.SceneName != SceneManager.GetActiveScene().name)
        {
            PlayerData pd = new PlayerData(this);
            this.PlayerData = pd;
            SaveManager.SavePlayer(pd);

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
        AnimatorVelocityHash = Animator.StringToHash("Velocity");
        AnimatorSpeedHash = Animator.StringToHash("SpeedMultiplier");
        //AnimatorVelocityHash = Animator.StringToHash("MoveX");
        //AnimatorVelocityHash = Animator.StringToHash("MoveZ");

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
        //controls.Player.BulletTimeInput.performed += TimeManager.EnableBulletTime;
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
            BulletTimeAudioSource.Pause();
            Debug.Log(Anim);
            Anim.SetFloat(AnimatorSpeedHash, TimeManager.PlayerCurrentSpeed);

            PauseCanvas.gameObject.SetActive(true);
        }
        else
        {
            TimeManager.DisablePause();
            Debug.Log(Anim);
            Anim.SetFloat(AnimatorSpeedHash, TimeManager.PlayerCurrentSpeed);
            if (TimeManager.IsBulletTimeActive)
            {
                BulletTimeAudioSource.UnPause();
            }
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
            Mixer.audioMixer.SetFloat("Pitch", 1f);
            BulletTimeAudioSource.Stop();
        }
        if (BulletTimeAudioSource.isPlaying)
        {
            BulletTimeAudioSource.pitch = 1.5f - (GetComponent<EllenActionPoints>().AP_Value / GetComponent<EllenActionPoints>().MaxAp) * 0.8f;
        }


        MoveRelativeToCameraRotation();
        //isGrounded = IsGroundedTest();
        GravityAndJumpUpdate();
        //Anim.SetBool("isGrounded", isGrounded);

        // Debug.LogWarning(GlobalVariables.PlayerVelocityAuto + " ..... " + characterController.velocity + " #### " + MeasuredVelocity) ;
    }
    void GravityAndJumpUpdate()
    {

        if (GroundCheckCooldown > 0f)
        {
            GroundCheckCooldown -= Time.deltaTime;
            GroundedCollider.Disable();

            //isGrounded = false;
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
            //Anim.SetBool("isGrounded", true);
            if (!isAiming)
                Anim.applyRootMotion = true;
        }
        else
        { //i am in the air
            GroundedCollider.SwitchToSmall();

            playerVel.x = MovementVector.x * SpeedInAir * speedAirMulti;
            playerVel.z = MovementVector.z * SpeedInAir * speedAirMulti;
        }

        if (jumpPressed && isGrounded)
        { // i am grounded and i want to jump
            isGrounded = false;
            GlobalVariables.IsPlayerGrounded = isGrounded;
            GroundedCollider.Disable();
            GroundCheckCooldown = 0.2f;
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
        if (GroundCheckCooldown > 0)
        {
            GroundCheckCooldown = GroundCheckCooldown - Time.deltaTime;
            return false;
        }
        else
        {

            Ray groundedTest = new Ray(this.transform.position + Vector3.up * 0.7f, Vector3.up * -0.5f);
            Debug.DrawRay(this.transform.position + Vector3.up, Vector3.down, Color.red, 1f);
            return Physics.SphereCast(groundedTest, 0.7f, 0.1f, SphereCastLayers);
        }
    }
    public void ManageBulletTimePlayerSide(InputAction.CallbackContext ctx)
    {

        if (EllenAp.Cooldown > 0f || TimeManager.IsGamePaused)
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

        Anim.SetFloat(AnimatorSpeedHash, TimeManager.PlayerCurrentSpeed);
    }
    void MoveRelativeToCameraRotation()
    {
        Vector3 fromAbsoluteToRelative = cameraQuatForMovement * new Vector3(direction.x, 0, direction.y);
        MovementVector = fromAbsoluteToRelative;
        GlobalVariables.PlayerVelocityGrounded = MovementVector;
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
            characterController.Move(MovementVector * PlayerSpeed * SpeedWhileAiming);

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
        if (!isAiming)
        {
            Anim.SetBool("Shift", true);
            speedAirMulti = SpeedInAirMultiplier;
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
