using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerZombieController : AIController<RunnerEnemyData>
{
    protected override void OnAttack()
    {
        // Don't allow runners to turn around during their attack (they will go to Idle after their attack goes on cooldown)
        MovementScript.SetCanFlip(false);
        
        // Runners will play attack animation
        base.OnAttack();
        
        // Runners will check every frame if their hitBox is enabled and hit something
        AttackScript.InitiateAttack();
    }
}
