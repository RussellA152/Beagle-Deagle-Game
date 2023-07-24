using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAnimationHandler : MonoBehaviour
{
    protected Animator Animator;
    
    protected int IsIdle;
    protected int IsMoving;
    //protected int IsAttacking;
    protected int Killed;

    private float _movementAnimationSpeed = 1f;
    private  int _movementSpeed = Animator.StringToHash("movementSpeed");

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        // All entities (player and enemy) have an idle, move, attack, and death animation
        IsIdle = Animator.StringToHash("isIdle");
        IsMoving = Animator.StringToHash("isMoving");
        Killed = Animator.StringToHash("killed");
        _movementSpeed = Animator.StringToHash("movementSpeed");
        
    }

    protected virtual void OnEnable()
    {
        // Reset animation movement speed
        Animator.SetFloat(_movementSpeed, 1f);
    }


    public virtual void PlayIdleAnimation()
    {
        // When idle, set all other bools to false
        Animator.SetBool(IsIdle, true);
        
        Animator.SetBool(IsMoving, false);

    }
    public virtual void PlayMoveAnimation()
    {
        // When chasing, set all other bools to false
        Animator.SetBool(IsMoving, true);
        
        Animator.SetBool(IsIdle, false);
    }
    
    public virtual void PlayDeathAnimation()
    {
        // Trigger death animation once, then set all bools to false
        Animator.SetTrigger(Killed);
        Animator.SetBool(IsIdle, false);
        Animator.SetBool(IsMoving, false);

    }
    public void SetMovementAnimationSpeed(float movementSpeedModifier)
    {
        _movementAnimationSpeed += movementSpeedModifier;
        
        Animator.SetFloat(_movementSpeed, _movementAnimationSpeed);
    }
    
}
