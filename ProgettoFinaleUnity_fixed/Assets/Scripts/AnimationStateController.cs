using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    int isDrawingTheGunHash;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isDrawingTheGunHash = Animator.StringToHash("isDrawingTheGun");
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        bool isDrawingTheGun = animator.GetBool(isDrawingTheGunHash);
        bool forwardPressed = this.GetComponent<CharacterController>().velocity.magnitude > 0;
        bool runPressed = Input.GetKey("left shift");
        bool isDrawingTheGunPressed = Input.GetKey("q");
        if (!isWalking && forwardPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }
        if (isWalking && !forwardPressed)
        {
            animator.SetBool(isWalkingHash, false);
        }
        if (!isRunning && (forwardPressed && runPressed))
        {
            animator.SetBool(isRunningHash, true);
        }
        if (isRunning && (!forwardPressed || !runPressed))
        {
            animator.SetBool(isRunningHash, false);
        }
        if (isDrawingTheGunPressed)
        {
            animator.SetBool(isDrawingTheGunHash, true);
        }
        if (!isDrawingTheGunPressed)
        {
            animator.SetBool(isDrawingTheGunHash, false);
        }


    }
}
