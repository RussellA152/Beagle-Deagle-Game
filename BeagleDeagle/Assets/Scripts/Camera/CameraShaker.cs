using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private CinemachineImpulseSource _impulseSource;
    
    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    

    public void ShakePlayerCamera(ScreenShakeData screenShakeData)
    {
        if (PlayerCamera.Instance != null && screenShakeData != null)
        {
            PlayerCamera.Instance.ScreenShakeFromProfile(screenShakeData, _impulseSource);
        }
        else
        {
            Debug.Log("Player Camera doesn't exist or no screen shake data! Cannot shake camera.");
        }
    }
}
