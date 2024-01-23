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

    private Camera _mainCamera;

    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private CinemachineImpulseListener _impulseListener;
    private CinemachineImpulseDefinition _impulseDefinition;

    private int _activeScreenShakes = 0;
    
    private void Awake()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _impulseListener = GetComponent<CinemachineImpulseListener>();

        if (Instance == null)
            Instance = this;
        
        _mainCamera = Camera.main;

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
        float randomX = GetRandomShakeValue(screenShakeData.minVelocityX, screenShakeData.maxVelocityX);
        float randomY = GetRandomShakeValue(screenShakeData.minVelocityY, screenShakeData.maxVelocityY);
        
        impulseSource.m_DefaultVelocity = new Vector3(randomX, randomY, 0f);
        
        _impulseDefinition.m_CustomImpulseShape = screenShakeData.impulseCurve;
        
        // Change the impulse listener settings
        _impulseListener.m_ReactionSettings.m_AmplitudeGain = screenShakeData.listenerAmplitude;
        _impulseListener.m_ReactionSettings.m_FrequencyGain = screenShakeData.listenerFrequency;
        _impulseListener.m_ReactionSettings.m_Duration = screenShakeData.listenerDuration;
    }
    
    ///-///////////////////////////////////////////////////////////
    /// Return either the minvalue or maxValue, no in between.
    /// 
    private float GetRandomShakeValue(float minValue, float maxValue)
    {
        return Random.Range(0, 2) * (maxValue - minValue + 1) + minValue;
    }

    public bool IsTransformOffCameraView(Transform transform)
    {
        Vector3 viewPos = _mainCamera.WorldToViewportPoint(transform.position);
        
        if ((viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1))
        {
            return true;
        }

        return false;

    }

    public Vector2 GetScreenCenter()
    {
        return new Vector2(Screen.width / 2, Screen.height / 2);
    }

    public Camera GetMainCamera()
    {
        return _mainCamera;
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

// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Cinemachine;
// using UnityEngine;
// using Random = UnityEngine.Random;
//
// public class PlayerCamera : MonoBehaviour
// {
//     public static PlayerCamera Instance;
//     [SerializeField] private PlayerEvents playerEvents;
//
//     private Camera mainCamera;
//
//     private CinemachineVirtualCamera _cinemachineVirtualCamera;
//     private CinemachineImpulseListener _impulseListener;
//     private CinemachineImpulseDefinition _impulseDefinition;
//     
//     
//     
//
//     private void Awake()
//     {
//         _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
//         _impulseListener = GetComponent<CinemachineImpulseListener>();
//
//         if (Instance == null)
//             Instance = this;
//         
//         mainCamera = Camera.main;
//         
//     }
//
//     private void OnEnable()
//     {
//         playerEvents.givePlayerGameObject += SetCameraFollowTarget;
//         
//     }
//
//     private void OnDisable()
//     {
//         playerEvents.givePlayerGameObject += SetCameraFollowTarget;
//     }
//
//     public void ScreenShakeFromProfile(ScreenShakeData screenShakeData, CinemachineImpulseSource impulseSource)
//     {
//         // Apply settings
//         SetupScreenShakeSettings(screenShakeData, impulseSource);
//         
//         // Screen shake
//         impulseSource.GenerateImpulseWithForce(screenShakeData.impactForce);
//     }
//
//     private void SetupScreenShakeSettings(ScreenShakeData screenShakeData, CinemachineImpulseSource impulseSource)
//     {
//         _impulseDefinition = impulseSource.m_ImpulseDefinition;
//         
//         // Change the impulse source settings
//         _impulseDefinition.m_ImpulseDuration = screenShakeData.impactTime;
//         
//         // Get a random velocity
//         float randomX = GetRandomShakeValue(screenShakeData.minVelocityX, screenShakeData.maxVelocityX);
//         float randomY = GetRandomShakeValue(screenShakeData.minVelocityY, screenShakeData.maxVelocityY);
//         
//         impulseSource.m_DefaultVelocity = new Vector3(randomX, randomY, 0f);
//         
//         _impulseDefinition.m_CustomImpulseShape = screenShakeData.impulseCurve;
//         
//         // Change the impulse listener settings
//         _impulseListener.m_ReactionSettings.m_AmplitudeGain = screenShakeData.listenerAmplitude;
//         _impulseListener.m_ReactionSettings.m_FrequencyGain = screenShakeData.listenerFrequency;
//         _impulseListener.m_ReactionSettings.m_Duration = screenShakeData.listenerDuration;
//     }
//
//     ///-///////////////////////////////////////////////////////////
//     /// Return either the minvalue or maxValue, no in betweens
//     /// 
//     private float GetRandomShakeValue(float minValue, float maxValue)
//     {
//         return Random.Range(0, 2) * (maxValue - minValue + 1) + minValue;
//     }
//
//     public bool IsTransformOffCameraView(Transform transform)
//     {
//         Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);
//         
//         if ((viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1))
//         {
//             return true;
//         }
//
//         return false;
//
//     }
//
//     public Vector2 GetScreenCenter()
//     {
//         return new Vector2(Screen.width / 2, Screen.height / 2);
//     }
//
//     public Camera GetMainCamera()
//     {
//         return mainCamera;
//     }
//
//     ///-///////////////////////////////////////////////////////////
//     /// Tell the cinemachine camera component that it should look at and follow the player
//     /// gameObject
//     /// 
//     private void SetCameraFollowTarget(GameObject pGameObject)
//     {
//         _cinemachineVirtualCamera.LookAt = pGameObject.transform;
//         _cinemachineVirtualCamera.Follow = pGameObject.transform;
//
//     }
// }
