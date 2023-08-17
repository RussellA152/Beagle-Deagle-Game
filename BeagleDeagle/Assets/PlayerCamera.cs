using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private PlayerEvents playerEvents;

    private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private void Awake()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        playerEvents.givePlayerGameObject += SetCameraFollowTarget;
    }

    private void OnDisable()
    {
        playerEvents.givePlayerGameObject += SetCameraFollowTarget;
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
