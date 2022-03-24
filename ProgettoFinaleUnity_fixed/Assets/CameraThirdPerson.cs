using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThirdPerson : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform CameraReference;
    Vector2 previousMousePos = Vector2.zero;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        CameraReference.rotation *= Quaternion.AngleAxis(Input.mousePosition.x, Vector3.up);
    }
}
