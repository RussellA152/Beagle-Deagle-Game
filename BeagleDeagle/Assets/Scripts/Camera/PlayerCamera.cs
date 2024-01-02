using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance;
    [SerializeField] private PlayerEvents playerEvents;

    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private CinemachineImpulseListener _impulseListener;
    private CinemachineImpulseDefinition _impulseDefinition;
    

    private void Awake()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _impulseListener = GetComponent<CinemachineImpulseListener>();

        if (Instance == null)
            Instance = this;
        
    }

    private void OnEnable()
    {
        playerEvents.givePlayerGameObject += SetCameraFollowTarget;
    }

    private void OnDisable()
    {
        playerEvents.givePlayerGameObject += SetCameraFollowTarget;
    }

    public void ScreenShakeFromProfile(ScreenShakeData screenShakeData, CinemachineImpulseSource impulseSource)
    {
        // Apply settings
        SetupScreenShakeSettings(screenShakeData, impulseSource);
        
        // Screen shake
        impulseSource.GenerateImpulseWithForce(screenShakeData.impactForce);
    }

    private void SetupScreenShakeSettings(ScreenShakeData screenShakeData, CinemachineImpulseSource impulseSource)
    {
        _impulseDefinition = impulseSource.m_ImpulseDefinition;
        
        // Change the impulse source settings
        _impulseDefinition.m_ImpulseDuration = screenShakeData.impactTime;
        
        // Get a random velocity
        float randomX = Random.Range(screenShakeData.minVelocityX, screenShakeData.maxVelocityX);
        float randomY = Random.Range(screenShakeData.minVelocityY, screenShakeData.maxVelocityY);
        
        impulseSource.m_DefaultVelocity = new Vector3(randomX, randomY, 0f);
        
        _impulseDefinition.m_CustomImpulseShape = screenShakeData.impulseCurve;
        
        // Change the impulse listener settings
        _impulseListener.m_ReactionSettings.m_AmplitudeGain = screenShakeData.listenerAmplitude;
        _impulseListener.m_ReactionSettings.m_FrequencyGain = screenShakeData.listenerFrequency;
        _impulseListener.m_ReactionSettings.m_Duration = screenShakeData.listenerDuration;
    }

    ///-///////////////////////////////////////////////////////////
    /// Tell the cinemachine camera component that it should look at and follow the player
    /// gameObject
    /// 
    private void SetCameraFollowTarget(GameObject pGameObject)
    {
        _cinemachineVirtualCamera.LookAt = pGameObject.transform;
        _cinemachineVirtualCamera.Follow = pGameObject.transform;

    }
}
