using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BulletTimeTest : MonoBehaviour
{
    public GameObject Player;
    private PlayerControllerSecondVersion PCSV;
    Controls playerInput;
    private float Speed = 50f;
    // Start is called before the first frame update
    void Start()
    {
        PCSV = Player.GetComponent<PlayerControllerSecondVersion>();
        playerInput = PCSV.Controls;
        playerInput.Player.BulletTimeInput.performed += SetSpeed;
    }

    void SetSpeed(InputAction.CallbackContext context)
    {

    }
    // Update is called once per frame
    void Update()
    {

        transform.Rotate(new Vector3(0, Speed * Time.deltaTime, 0));
    }
}
