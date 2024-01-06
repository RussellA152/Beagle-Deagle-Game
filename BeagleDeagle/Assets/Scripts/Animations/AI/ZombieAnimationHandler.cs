using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationHandler : BasicAnimationHandler, IEnemyDataUpdatable
{
    // Enemy scriptable object contains a runtime animator controller
    [SerializeField] private EnemyData _enemyScriptableObject;

    private Color _originalColor;

    // Enemies can be stunned (unlike player), so we need to play an animation for that
    private int _isStunned;
    
    private int _isAttacking;
    private float _attackAnimationSpeed = 1f;
    private int _attackSpeed = Animator.StringToHash("attackSpeed");

    protected override void Start()
    {
        base.Start();
     
        // TODO: Might move this to base class 
        animator.runtimeAnimatorController = _enemyScriptableObject.animatorController;
        
        _isAttacking = Animator.StringToHash("isAttacking");
        _isStunned = Animator.StringToHash("isStunned");
        _attackSpeed = Animator.StringToHash("attackSpeed");

        _originalColor = SpriteRenderer.color;

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        // Reset animation movement speed
        animator.SetFloat(_attackSpeed, 1f);

        // Change the color of this sprite
        // TODO: Will eventually just use new sprites instead of colors
        SpriteRenderer.color = _enemyScriptableObject.spriteColor;

    }

    public override void PlayIdleAnimation()
    {
        // When idle, set all other bools to false
        base.PlayIdleAnimation();
        animator.SetBool(_isAttacking, false);
        animator.SetBool(_isStunned, false);
        
        
    }
    public override void PlayMoveAnimation()
    {
        base.PlayMoveAnimation();
        animator.SetBool(_isAttacking, false);
        animator.SetBool(_isStunned, false);
        
    }

    public void PlayAttackAnimation()
    {
        // When attacking, set all other bools to false
        animator.SetBool(_isAttacking, true);
        
        animator.SetBool(IsIdle, false);
        animator.SetBool(IsMoving, false);
        
        animator.SetBool(_isStunned, false);
        
    }

    public void PlayStunAnimation()
    {
        // When stunned, set all other bools to false
        animator.SetBool(_isStunned, true);
        
        animator.SetBool(IsIdle, false);
        animator.SetBool(IsMoving, false);
        animator.SetBool(_isAttacking, false);
    }

    public override void PlayDeathAnimation()
    {
        // Trigger death animation once, then set all bools to false
        base.PlayDeathAnimation();
        animator.SetBool(_isAttacking, false);
        animator.SetBool(_isStunned, false);

    }
    
    public void SetAttackAnimationSpeed(float attackSpeedModifier)
    {
        _attackAnimationSpeed += attackSpeedModifier;
        
        animator.SetFloat(_attackSpeed, _attackAnimationSpeed);
    }

    public void UpdateScriptableObject(EnemyData scriptableObject)
    {
        _enemyScriptableObject = scriptableObject;
        
        // TODO: Will eventually just use new sprites instead of colors
        SpriteRenderer.color = _enemyScriptableObject.spriteColor;
    }
}
