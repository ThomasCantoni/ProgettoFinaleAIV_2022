using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeScript : MonoBehaviour
{
    [SerializeField]
    public List<CinemachineVirtualCamera> Cameras;
    public float Frequency=1f, Amplitude=1f,DecreaseFactor=1f;
    private float initialFrequency = 1f, initialAmplitude = 1f;

    private float initialDuration = 1, currentDuration = 0;
    public float Duration 
    {  get
        {
            return currentDuration;
        }
        set
        {
            initialDuration = value;
        }
    }
    private void Start()
    {
        enabled = false;
    }
    public void ApplyShake(float duration=1f,float frequency = 1f,float amplitude=1f)
    {
        enabled = true;
        initialDuration = duration;
        currentDuration = initialDuration;
        initialAmplitude = amplitude;
        initialFrequency = frequency;
    }
    public void Update()
    {
        
        if(currentDuration > 0)
        {
            currentDuration -= Time.deltaTime;
            
            Frequency = initialFrequency * (currentDuration/initialDuration);
            Amplitude = initialAmplitude * (currentDuration / initialDuration);
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
