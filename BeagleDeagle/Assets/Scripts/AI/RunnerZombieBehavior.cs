using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerZombieBehavior : AIBehavior<RunnerEnemyData>
{
    private int _isIdle;
    private int _isMoving;
    private int _isStunned;
    private int _isAttacking;
    private int _killed;

    protected override void Start()
    {
        // All animations that runner zombies use
        _isIdle = Animator.StringToHash("isIdle");
        _isMoving = Animator.StringToHash("isMoving");
        _isStunned = Animator.StringToHash("isStunned");
        _isAttacking = Animator.StringToHash("isAttacking");
        _killed = Animator.StringToHash("killed");
        
        base.Start();
    }
    protected override void OnIdle()
    {
        // When idle, set all other bools to false
        animator.SetBool(_isIdle, true);
        
        animator.SetBool(_isMoving, false);
        animator.SetBool(_isStunned, false);
        animator.SetBool(_isAttacking, false);
        
        base.OnIdle();
        
    }

    protected override void OnChase()
    {
        // When chasing, set all other bools to false
        animator.SetBool(_isMoving, true);
        
        animator.SetBool(_isIdle, false);
        animator.SetBool(_isAttacking, false);
        animator.SetBool(_isStunned, false);
        
        base.OnChase();
    }

    protected override void OnAttack()
    {
        // When attacking, set all other bools to false
        animator.SetBool(_isAttacking, true);
        
        animator.SetBool(_isIdle, false);
        animator.SetBool(_isMoving, false);
        animator.SetBool(_isStunned, false);
        
        base.OnAttack();
    }

    protected override void OnStun()
    {
        // When stunned, set all other bools to false
        animator.SetBool(_isStunned, true);
        
        animator.SetBool(_isIdle, false);
        animator.SetBool(_isMoving, false);
        animator.SetBool(_isAttacking, false);
        
        base.OnStun();
    }

    protected override void OnDeath()
    {
        // Trigger death animation once, then set all bools to false
        animator.SetTrigger(_killed);
        
        animator.SetBool(_isIdle, false);
        animator.SetBool(_isMoving, false);
        animator.SetBool(_isAttacking, false);
        animator.SetBool(_isStunned, false);
        
        base.OnDeath();
    }
}
