using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerState state;
    
    [SerializeField] private PlayerEvents playerEvents;
    
    private PlayerInput _playerInput;
    
    [SerializeField] private CharacterData currentPlayerData;
    
    private TopDownMovement _movementScript;
    
    private PlayerHealth _healthScript;

    private PlayerAnimationHandler _animationHandlerScript;

    private Gun _gunScript;
    

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
        _playerInput = GetComponent<PlayerInput>();
        _movementScript = GetComponent<TopDownMovement>();
        _healthScript = GetComponent<PlayerHealth>();
        _animationHandlerScript = GetComponent<PlayerAnimationHandler>();
        _gunScript = GetComponentInChildren<Gun>();
    }

    private void OnEnable()
    {
        playerEvents.InvokeNewWeaponEvent(_gunScript.GetCurrentData());

        playerEvents.InvokeNewStatsEvent(currentPlayerData);

        playerEvents.InvokeGivePlayerInputComponentEvent(_playerInput);
        
        _movementScript.AllowMovement(true);
        _gunScript.AllowReload(true);
        _gunScript.AllowShoot(true);
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
                if (CheckIfAttacking())
                    state = PlayerState.Attacking;
                if (CheckIfMoving())
                    state = PlayerState.Moving;
                break;
            case PlayerState.Moving:
                _animationHandlerScript.PlayMoveAnimation();
                if (CheckIfIdle())
                    state = PlayerState.Idle;
                if (CheckIfAttacking())
                    state = PlayerState.Attacking;
                break;
            case PlayerState.Rolling:
                if (CheckIfIdle())
                    state = PlayerState.Idle;
                if (CheckIfAttacking())
                    state = PlayerState.Attacking;
                if (CheckIfMoving())
                    state = PlayerState.Moving;
                break;
            case PlayerState.Attacking:
                if (CheckIfIdle())
                    state = PlayerState.Idle;
                if (CheckIfMoving() && !CheckIfAttacking())
                    state = PlayerState.Moving;
                break;
            case PlayerState.Death:
                _movementScript.AllowMovement(false);
                _gunScript.AllowReload(false);
                _gunScript.AllowShoot(false);
                _animationHandlerScript.PlayDeathAnimation();
                //Destroy(this.gameObject);
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
    
    private bool CheckIfAttacking()
    {
        // Checking if the player is attacking with their weapon
        if(_gunScript == null)
        {
            Debug.Log("WEAPON MISSING!");
            return false;
        }
        else
        {
            return _gunScript.ActuallyShooting;
        }
        
    }

    private bool CheckIfDead()
    {
        // check if the player was killed
        return _healthScript.IsDead();
    }

}

