using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerState state;
    
    [Header("Event Systems")]
    [SerializeField] private PlayerEvents playerEvents;
    [SerializeField] private GameEvents gameEvents;
    
    [Header("Data to Use")]
    [SerializeField] private PlayerData currentPlayerData;
    
    [Header("Required Scripts")]
    private TopDownMovement _movementScript;
    private PlayerHealth _healthScript;
    private PlayerAnimationHandler _animationHandlerScript;
    private Gun _gunScript;
    private IUtilityUpdatable _utilityScript;
    private IUltimateUpdatable _ultimateScript;
    private IHasInput[] _allInputScripts;


    // States that a player can be in
    private enum PlayerState
    {
        // Not moving
        Idle,

        // Walking around
        Moving,

        // Player used "roll" ability
        Rolling,

        // Player is shooting or using their weapon in general
        Attacking,
        
        // Player was killed
        Death

    }

    private void Awake()
    {
        // Fetch all script dependencies
        _movementScript = GetComponent<TopDownMovement>();
        _healthScript = GetComponent<PlayerHealth>();
        _animationHandlerScript = GetComponent<PlayerAnimationHandler>();
        _gunScript = GetComponentInChildren<Gun>();

        _utilityScript = GetComponent<IUtilityUpdatable>();
        _ultimateScript = GetComponent<IUltimateUpdatable>();

        _allInputScripts = GetComponentsInChildren<IHasInput>();
        
    }

    private void OnEnable()
    {
        if(_gunScript == null)
        {
            Debug.Log("WEAPON MISSING!");
        }

        gameEvents.onGamePause += DisableAllPlayerInput;
        gameEvents.onGameResumeAfterPause += EnableAllPlayerInput;
    }

    private void OnDisable()
    {
        gameEvents.onGamePause -= DisableAllPlayerInput;
        gameEvents.onGameResumeAfterPause -= EnableAllPlayerInput;
    }

    private void Start()
    {
        state = PlayerState.Idle;
        
        // Tell all listeners that this is the player gameObject
        playerEvents.InvokeFindPlayer(gameObject);
        
        // Tell all listeners the current player stats/data they have
        playerEvents.InvokeNewStatsEvent(currentPlayerData);
        
        // Allow player to do all their abilities at the start of the game
        _movementScript.AllowMovement(true);
        _movementScript.AllowRotation(true);
        _gunScript.AllowShoot(true);
        _gunScript.AllowReload(true);
        _gunScript.AllowWeaponReceive(true);
        _utilityScript.AllowUtility(true);
        _ultimateScript.AllowUltimate(true);
    }

    private void Update()
    {
        // Switch to death state if the player's health reached 0
        if (CheckIfDead())
            state = PlayerState.Death;

        switch (state)
        {
            case PlayerState.Idle:
                
                _animationHandlerScript.PlayIdleAnimation();
                
                // Allow all movement and attacks
                _movementScript.AllowMovement(true);
                _movementScript.AllowRotation(true);
                _gunScript.AllowShoot(true);
                _utilityScript.AllowUtility(true);
                _ultimateScript.AllowUltimate(true);
                
                
                if (CheckIfAttacking())
                    state = PlayerState.Attacking;
                if (CheckIfMoving())
                    state = PlayerState.Moving;
                if (CheckIfRolling())
                    state = PlayerState.Rolling;
                break;
            
            case PlayerState.Moving:
                
                _animationHandlerScript.PlayMoveAnimation();
                
                // Allow all movement and attacks
                _movementScript.AllowMovement(true);
                _movementScript.AllowRotation(true);
                _gunScript.AllowShoot(true);
                _utilityScript.AllowUtility(true);
                _ultimateScript.AllowUltimate(true);
                
                if (CheckIfIdle())
                    state = PlayerState.Idle;
                if (CheckIfAttacking())
                    state = PlayerState.Attacking;
                if (CheckIfRolling())
                    state = PlayerState.Rolling;
                break;
            
            case PlayerState.Rolling:
                _movementScript.AllowRotation(false);
                _animationHandlerScript.PlayRollAnimation();
                
                // Allow movement input
                _movementScript.AllowMovement(true);
                // Don't allow player to rotate weapon or body

                // Don't allow any abilities or attacks
                _gunScript.AllowShoot(false);
                _utilityScript.AllowUtility(false);
                _ultimateScript.AllowUltimate(false);
                
                if (CheckIfIdle())
                    state = PlayerState.Idle;
                if (CheckIfAttacking())
                    state = PlayerState.Attacking;
                if (CheckIfMoving())
                    state = PlayerState.Moving;
                if (CheckIfRolling())
                    state = PlayerState.Rolling;
                break;
            
            case PlayerState.Attacking:
                
                // Allow all movement and attacks
                _movementScript.AllowMovement(true);
                _movementScript.AllowRotation(true);
                _gunScript.AllowShoot(true);
                _utilityScript.AllowUtility(true);
                _ultimateScript.AllowUltimate(true);
                
                if (CheckIfIdle())
                    state = PlayerState.Idle;
                if (CheckIfMoving() && !CheckIfAttacking())
                    state = PlayerState.Moving;
                if (CheckIfRolling())
                    state = PlayerState.Rolling;
                break;
            
            case PlayerState.Death:
                
                // Disable all movement, attacks, and abilities
                _movementScript.AllowMovement(false);
                _movementScript.AllowRotation(false);
                _movementScript.AllowRoll(false);
                _gunScript.AllowReload(false);
                _gunScript.AllowShoot(false);
                _utilityScript.AllowUtility(false);
                _ultimateScript.AllowUltimate(false);
                _animationHandlerScript.PlayDeathAnimation();
                break;
            
        }
    }

    private bool CheckIfIdle()
    {
        // If the player is not moving and not attacking
        return (!CheckIfMoving() && !CheckIfAttacking());
    }

    private bool CheckIfMoving()
    {
        // Checking if the player is pressing any movement keys (or moving the left stick)
        return ((Mathf.Abs(_movementScript.MovementInput.x) > 0f) || (Mathf.Abs(_movementScript.MovementInput.y) > 0f));
    }

    private bool CheckIfRolling()
    {
        return _movementScript.IsRolling;
    }
    
    private bool CheckIfAttacking()
    {
        // Checking if the player is attacking with their weapon
        return _gunScript.ActuallyShooting;
        
    }

    private bool CheckIfDead()
    {
        // Check if the player was killed
        return _healthScript.IsDead();
    }

    ///-///////////////////////////////////////////////////////////
    /// Enable/Reenable all input for player abilities (i.e shooting, reloading, moving, etc.)
    /// 
    private void EnableAllPlayerInput()
    {
        foreach (IHasInput inputScript in _allInputScripts)
        {
            inputScript.AllowInput(true);
        }
    }

    ///-///////////////////////////////////////////////////////////
    /// Disable all input for player abilities (i.e shooting, reloading, moving, etc.)
    /// 
    private void DisableAllPlayerInput()
    {
        foreach (IHasInput inputScript in _allInputScripts)
        {
            inputScript.AllowInput(false);
            
            //Debug.Log(inputScript);
        }
    }
    

}

