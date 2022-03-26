using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    public float Speed = 5;
    public Camera Camera;
    public GameObject BodyToMove;
    CharacterController toMove;
    Vector3 forceToAdd = Vector3.zero, fixedForward = Vector3.zero;
    float strafe = 0, vertical = 0, forward = 0;
    // Start is called before the first frame update
    void Start()
    {
        toMove = BodyToMove.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        forceToAdd = new Vector3();

        strafe = Input.GetAxis("Horizontal");
        forward = Input.GetAxis("Vertical");

        if (!toMove.isGrounded)
        {
            vertical += Physics.gravity.y * Time.deltaTime;

        }
        else
        {
            vertical = 0;
        }
        forceToAdd += Camera.transform.forward * forward;
        forceToAdd += Camera.transform.right * strafe;
        forceToAdd.Normalize();
        forceToAdd.y = vertical;

        fixedForward = forceToAdd;
        fixedForward.y = 0;


    }
    private void FixedUpdate()
    {
        toMove.Move(forceToAdd * Time.deltaTime * Speed);
        if (fixedForward.magnitude > 0)
        {

            toMove.transform.forward = Vector3.Lerp(toMove.transform.forward, fixedForward, 0.6f);

        }
        //toMove.attachedRigidbody.AddForce(forceToAdd, ForceMode.Impulse);

    }
}
