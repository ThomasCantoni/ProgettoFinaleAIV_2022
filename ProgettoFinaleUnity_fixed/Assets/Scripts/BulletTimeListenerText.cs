using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class BulletTimeListenerText : MonoBehaviour
{
    public PlayerControllerSecondVersion PCSV;
    private Controls controls;
    void Start()
    {
        controls = PCSV.Controls;
        controls.Player.BulletTimeInput.performed += Toggle;
        this.gameObject.SetActive(false);

    }

    void Toggle(InputAction.CallbackContext ctx)
    {
        this.gameObject.SetActive(!gameObject.activeSelf);
    }

}
