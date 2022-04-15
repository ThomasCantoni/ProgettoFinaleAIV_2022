using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeScript : MonoBehaviour
{
    [SerializeField]
    public List<CinemachineVirtualCamera> Cameras;
    public float DurationSeconds = 1f;
    public float Frequency=1f, Amplitude=1f,DecreaseFactor=1f;

    private void Start()
    {
        enabled = false;
    }
    public void ApplyShake(float duration=1f,float frequency = 1f,float amplitude=1f)
    {
        enabled = true;
        DurationSeconds = duration;
        Frequency = frequency;
        Amplitude = amplitude;
    }
    public void Update()
    {
        
        if(DurationSeconds >= 0)
        {
            DurationSeconds -= Time.deltaTime*DecreaseFactor;
            Frequency -= Time.deltaTime * DecreaseFactor;
            Amplitude -= Time.deltaTime * DecreaseFactor;
            for (int i = 0; i < Cameras.Count; i++)
            {
                CinemachineBasicMultiChannelPerlin noise = Cameras[i].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                noise.m_AmplitudeGain = Amplitude;
                noise.m_FrequencyGain = Frequency;
            }
        }
        else
        {
            for (int i = 0; i < Cameras.Count; i++)
            {
                CinemachineBasicMultiChannelPerlin noise = Cameras[i].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                noise.m_AmplitudeGain = 0;
                noise.m_FrequencyGain = 0;
            }
            enabled = false;
        }
    }

}
