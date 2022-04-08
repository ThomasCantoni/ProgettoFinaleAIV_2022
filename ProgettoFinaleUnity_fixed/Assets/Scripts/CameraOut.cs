using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraOut : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public float targetFOV;
    private void Start()
    {
        targetFOV = cam.m_Lens.FieldOfView + 30f;
        if (cam.m_Lens.FieldOfView >= 90f)
        {
            targetFOV = cam.m_Lens.FieldOfView - 20f;
        }
    }
    void Update()
    {
        cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, targetFOV, 0.035f);

    }
}
