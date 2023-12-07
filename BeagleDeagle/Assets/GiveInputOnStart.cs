using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

///-///////////////////////////////////////////////////////////
/// At start, invoke an event from PlayerEvents that gives the PlayerInput component to all listeners.
/// Attach this script to any gameObject that contains PlayerInput (ex. MenuInput gameObject in menu scenes, or Player gameObjects in game scenes).
/// 
public class GiveInputOnStart : MonoBehaviour
{
    private PlayerInput _playerInput;
    [SerializeField] private PlayerEvents playerEvents;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        playerEvents.InvokeFindPlayerInput(_playerInput);
    }
}
