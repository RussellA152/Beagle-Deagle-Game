using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : BasicAnimationHandler
{
    private int _isRolling;
    protected override void Start()
    {
        base.Start();
        _isRolling = Animator.StringToHash("isRolling");
    }

    public override void PlayIdleAnimation()
    {
        base.PlayIdleAnimation();
        animator.SetBool(_isRolling, false);
    }

    public override void PlayMoveAnimation()
    {
        base.PlayMoveAnimation();
        animator.SetBool(_isRolling, false);
    }

    public void PlayRollAnimation()
    {
        animator.SetBool(IsIdle, false);
        animator.SetBool(IsMoving, false);
        
        // When idle, set all other bools to false
        animator.SetBool(_isRolling, true);
        
    }

    public override void PlayDeathAnimation()
    {
        base.PlayDeathAnimation();
        animator.SetBool(_isRolling, false);
    }
}
