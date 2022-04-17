using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlatformDestroy : MonoBehaviour
{
    private float timer = 0;
    public float timerOut = 1f;
    public Animator anim;

    private void OnTriggerStay(Collider other)
    {
        PlayerControllerSecondVersion PCSV = other.GetComponent<PlayerControllerSecondVersion>();
        CinemachineVirtualCamera Camera = PCSV.ThirdPersonCamera;
        CinemachineBasicMultiChannelPerlin C = Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        C.m_AmplitudeGain = 1.2f;
        C.m_FrequencyGain = 1.2f;
        timer += Time.deltaTime;
        anim.enabled = true;
        if (timer >= timerOut)
        {
            this.transform.parent.gameObject.transform.position =(new Vector3(0,0,0));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerControllerSecondVersion PCSV = other.GetComponent<PlayerControllerSecondVersion>();
        CinemachineVirtualCamera Camera = PCSV.ThirdPersonCamera;
        CinemachineBasicMultiChannelPerlin C = Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        C.m_AmplitudeGain = 0f;
        C.m_FrequencyGain = 0f;
        Destroy(this.transform.parent.gameObject, 1f);
    }
}
