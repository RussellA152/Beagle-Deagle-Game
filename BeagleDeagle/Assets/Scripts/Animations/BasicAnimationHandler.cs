using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAnimationHandler : MonoBehaviour
{
    [SerializeField] 
    protected Animator animator;
    
    protected int _isIdle;
    protected int _isMoving;
    protected int _isAttacking;
    protected int _killed;

    private float _movementAnimationSpeed = 1f;
    private  int _movementSpeed = Animator.StringToHash("movementSpeed");

    protected virtual void Start()
    {
        // All entities (player and enemy) have an idle, move, attack, and death animation
        _isIdle = Animator.StringToHash("isIdle");
        _isMoving = Animator.StringToHash("isMoving");
        _isAttacking = Animator.StringToHash("isAttacking");
        _killed = Animator.StringToHash("killed");
        _movementSpeed = Animator.StringToHash("movementSpeed");
        
    }

    protected virtual void OnEnable()
    {
        // Reset animation movement speed
        animator.SetFloat(_movementSpeed, 1f);
    }


    public virtual void PlayIdleAnimation()
    {
        // When idle, set all other bools to false
        animator.SetBool(_isIdle, true);
        
        animator.SetBool(_isMoving, false);
        //animator.SetBool(_isStunned, false);
        animator.SetBool(_isAttacking, false);

    }
    public virtual void PlayMoveAnimation()
    {
        // When chasing, set all other bools to false
        animator.SetBool(_isMoving, true);
        
        animator.SetBool(_isIdle, false);
        animator.SetBool(_isAttacking, false);
    }

    public virtual void PlayAttackAnimation()
    {
        // When attacking, set all other bools to false
        animator.SetBool(_isAttacking, true);
        
        animator.SetBool(_isIdle, false);
        animator.SetBool(_isMoving, false);

    }
    public virtual void PlayDeathAnimation()
    {
        // Trigger death animation once, then set all bools to false
        animator.SetTrigger(_killed);
        animator.SetBool(_isIdle, false);
        animator.SetBool(_isMoving, false);
        animator.SetBool(_isAttacking, false);
        
    }
    public void SetMovementAnimationSpeed(float movementSpeedModifier)
    {
        _movementAnimationSpeed += movementSpeedModifier;
        
        animator.SetFloat(_movementSpeed, _movementAnimationSpeed);
    }
    
}
