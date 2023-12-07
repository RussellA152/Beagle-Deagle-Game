using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControllerTypeManager : MonoBehaviour
{
    public static PlayerControllerTypeManager Instance;

    public PlayerEvents playerEvents;
    
    private PlayerInput _playerInput;

    private ControllerType _currentControllerType;

    // Invoke this event when the player has changed their current controller (Keyboard/Mouse, Xbox, or Playstation)
    public Action<ControllerType> OnPlayerChangedController;

    private void Awake()
    {
        // Create singleton instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
        
    }

    private void OnEnable()
    {
        playerEvents.givePlayerInput += FindPlayerInput;
    }

    private void OnDisable()
    {
        if(_playerInput != null)
            _playerInput.onControlsChanged -= ControlsChanged;
    }
    

    private void FindPlayerInput(PlayerInput playerInput)
    {
        // Find the player input component in the level
        _playerInput = playerInput;
        
        _playerInput.onControlsChanged += ControlsChanged;
        
        // Initialize controls (most likely keyboard at first)
        ControlsChanged(_playerInput);
    }
    
    ///-///////////////////////////////////////////////////////////
    /// All devices that the user can play the game with and have 
    /// button prompts displayed on the HUD
    /// 
    public enum ControllerType
    {
        Keyboard,
        Xbox,
        Playstation
    }

    ///-///////////////////////////////////////////////////////////
    /// When the user starts playing with a different controller, tell all subscribers
    /// that the user changed to either a keyboard, xbox gamepad, or playstation gamepad
    /// 
    private void ControlsChanged(PlayerInput playerInput)
    {
        Debug.Log("invoked!");
        switch (playerInput.currentControlScheme)
        {
            case "Keyboard":
                _currentControllerType = ControllerType.Keyboard;
                Cursor.visible = true;
                Debug.Log("Swapped to keyboard!");
                break;
            case "Gamepad":
                Gamepad currentGamepad = Gamepad.current;
                if (currentGamepad != null)
                {
                    string deviceName = currentGamepad.device.displayName;
                    
                    if (deviceName.Contains("Xbox"))
                    {
                        _currentControllerType = ControllerType.Xbox;
                        Cursor.visible = false;
                        Debug.Log("Swapped to Xbox gamepad!");
                    }
                    else if (deviceName.Contains("DualShock") || deviceName.Contains("DualSense"))
                    {
                        _currentControllerType = ControllerType.Playstation;
                        Cursor.visible = false;
                        Debug.Log("Swapped to PlayStation gamepad!");
                    }
                    else
                    {
                        _currentControllerType = ControllerType.Xbox;
                        Cursor.visible = false;
                        Debug.Log($"Swapped to gamepad: {deviceName}");
                    }
                }
                else
                {
                    Cursor.visible = true;
                    Debug.Log("A gamepad was disconnected.");
                }
                break;
        }
        
        OnPlayerChangedController?.Invoke(_currentControllerType);
    }
}
