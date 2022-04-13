using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class test : MonoBehaviour
{
    Vector3 movementInput;
    Vector2 playerMouseInput;
    [SerializeField] Transform PlayerCamera;
    [SerializeField] Rigidbody PlayerBody;
    [Space]
    float Speed;
    float Sensitivity;
    float jumpforce;
    Controls c;
    private void Start()
    {
        c = new Controls();
        
        c.Player.Movement.performed += ctx => MovePlayer(ctx.ReadValue<Vector2>());
    }
     void MovePlayer(Vector2 dir)
    {
        
        movementInput = transform.InverseTransformDirection(dir);
        PlayerBody.velocity += new Vector3(movementInput.x, PlayerBody.velocity.y, movementInput.z);
    }


}
