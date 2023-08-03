using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerState state;
    
    [SerializeField] private PlayerEvents playerEvents;
    
    [Header("Data to Use")]
    [SerializeField] private PlayerData currentPlayerData;
    
    [Header("Required Scripts")]
    private TopDownMovement _movementScript;
    private PlayerHealth _healthScript;
    private PlayerAnimationHandler _animationHandlerScript;
    private Gun _gunScript;
    private IUtilityUpdatable _utilityScript;
    private IUltimateUpdatable _ultimateScript;
    

    // states that an enemy can be in
    private enum PlayerState
    {
        // not moving
        Idle,

        // walking around
        Moving,

        // player used "roll" ability
        Rolling,

        // player is shooting or using their weapon in general
        Attacking,
        
        // player was killed
        Death

    }

    private void Awake()
    {
        _movementScript = GetComponent<TopDownMovement>();
        _healthScript = GetComponent<PlayerHealth>();
        _animationHandlerScript = GetComponent<PlayerAnimationHandler>();
        _gunScript = GetComponentInChildren<Gun>();

        _utilityScript = GetComponent<IUtilityUpdatable>();
        _ultimateScript = GetComponent<IUltimateUpdatable>();
    }

    private void OnEnable()
    {
        playerEvents.InvokeNewWeaponEvent(_gunScript.GetCurrentData());

        playerEvents.InvokeNewStatsEvent(currentPlayerData);
        
        _movementScript.AllowMovement(true);
        
        _gunScript.AllowReload(true);
        _gunScript.AllowShoot(true);
        
        _utilityScript.AllowUtility(true);
        _ultimateScript.AllowUltimate(true);
        
        if(_gunScript == null)
        {
            Debug.Log("WEAPON MISSING!");
        }
    }


    private void Start()
    {
        state = PlayerState.Idle;
        
    }

    private void Update()
    {
        if (CheckIfDead())
            state = PlayerState.Death;
        

        switch (state)
        {
            case PlayerState.Idle:
                
                _animationHandlerScript.PlayIdleAnimation();
                
                _movementScript.AllowMovement(true);
                _gunScript.AllowReload(true);
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
                if (CheckIfIdle())
                    state = PlayerState.Idle;
                if (CheckIfAttacking())
                    state = PlayerState.Attacking;
                if (CheckIfRolling())
                    state = PlayerState.Rolling;
                break;
            
            case PlayerState.Rolling:
                _animationHandlerScript.PlayRollAnimation();
                
                _movementScript.AllowMovement(true);
                _movementScript.AllowRotation(false);
                _gunScript.AllowReload(false);
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
        // if the player is not moving and not attacking
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
        // check if the player was killed
        return _healthScript.IsDead();
    }

}

