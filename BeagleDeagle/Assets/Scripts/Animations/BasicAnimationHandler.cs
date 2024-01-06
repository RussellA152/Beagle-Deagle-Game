using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BasicAnimationHandler : MonoBehaviour
{
    [HideInInspector] public Animator animator;

    protected SpriteRenderer SpriteRenderer;
    
    protected int IsIdle;
    protected int IsMoving;
    protected int Killed;

    private float _movementAnimationSpeed = 1f;
    private  int _movementSpeed = Animator.StringToHash("movementSpeed");

    private void Awake()
    {
        animator = GetComponent<Animator>();

        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
        animator.SetFloat(_movementSpeed, 1f);
    }


    public virtual void PlayIdleAnimation()
    {
        // When idle, set all other bools to false
        animator.SetBool(IsIdle, true);
        
        animator.SetBool(IsMoving, false);

    }
    public virtual void PlayMoveAnimation()
    {
        // When chasing, set all other bools to false
        animator.SetBool(IsMoving, true);
        
        animator.SetBool(IsIdle, false);
    }
    
    public virtual void PlayDeathAnimation()
    {
        // Trigger death animation once, then set all bools to false
        animator.SetTrigger(Killed);
        animator.SetBool(IsIdle, false);
        animator.SetBool(IsMoving, false);

    }
    public void SetMovementAnimationSpeed(float movementSpeedModifier)
    {
        _movementAnimationSpeed += movementSpeedModifier;
        
        animator.SetFloat(_movementSpeed, _movementAnimationSpeed);
    }
    
}
