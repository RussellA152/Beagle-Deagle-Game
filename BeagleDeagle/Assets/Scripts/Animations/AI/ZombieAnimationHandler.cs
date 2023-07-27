using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationHandler : BasicAnimationHandler, IEnemyDataUpdatable
{
    // Enemy scriptable object contains a runtime animator controller
    [SerializeField] private EnemyData _enemyScriptableObject;
    
    // Enemies can be stunned (unlike player), so we need to play an animation for that
    private int _isStunned;
    
    private int _isAttacking;
    private float _attackAnimationSpeed = 1f;
    private int _attackSpeed = Animator.StringToHash("attackSpeed");

    protected override void Start()
    {
        base.Start();
     
        // TODO: Might move this to base class 
        Animator.runtimeAnimatorController = _enemyScriptableObject.animatorController;
        
        _isAttacking = Animator.StringToHash("isAttacking");
        _isStunned = Animator.StringToHash("isStunned");
        _attackSpeed = Animator.StringToHash("attackSpeed");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        // Reset animation movement speed
        Animator.SetFloat(_attackSpeed, 1f);
    }

    public override void PlayIdleAnimation()
    {
        // When idle, set all other bools to false
        base.PlayIdleAnimation();
        Animator.SetBool(_isAttacking, false);
        Animator.SetBool(_isStunned, false);
        
        
    }
    public override void PlayMoveAnimation()
    {
        base.PlayMoveAnimation();
        Animator.SetBool(_isAttacking, false);
        Animator.SetBool(_isStunned, false);
        
    }

    public void PlayAttackAnimation()
    {
        // When attacking, set all other bools to false
        Animator.SetBool(_isAttacking, true);
        
        Animator.SetBool(IsIdle, false);
        Animator.SetBool(IsMoving, false);
        
        Animator.SetBool(_isStunned, false);
        
    }

    public void PlayStunAnimation()
    {
        // When stunned, set all other bools to false
        Animator.SetBool(_isStunned, true);
        
        Animator.SetBool(IsIdle, false);
        Animator.SetBool(IsMoving, false);
        Animator.SetBool(_isAttacking, false);
    }

    public override void PlayDeathAnimation()
    {
        // Trigger death animation once, then set all bools to false
        base.PlayDeathAnimation();
        Animator.SetBool(_isAttacking, false);
        Animator.SetBool(_isStunned, false);

    }
    
    public void SetAttackAnimationSpeed(float attackSpeedModifier)
    {
        _attackAnimationSpeed += attackSpeedModifier;
        
        Animator.SetFloat(_attackSpeed, _attackAnimationSpeed);
    }

    public void UpdateScriptableObject(EnemyData scriptableObject)
    {
        _enemyScriptableObject = scriptableObject;
    }
}
