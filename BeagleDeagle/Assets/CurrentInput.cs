using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CurrentInput : MonoBehaviour
{
    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _playerInput.onControlsChanged += ControlsChanged;
    }

    private void OnDisable()
    {
        _playerInput.onControlsChanged -= ControlsChanged;
    }

    // private void Update()
    // {
    //     Debug.Log(_playerInput.currentControlScheme);
    // }

    private void ControlsChanged(PlayerInput playerInput)
    {
        Debug.Log(playerInput.currentControlScheme);
    }
    
}
