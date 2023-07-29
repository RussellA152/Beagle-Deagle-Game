using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerZombieController : AIController<RunnerEnemyData>
{
    protected override void OnAttack()
    {
        // Runners will play attack animation
        base.OnAttack();
        
        // Runners will check every frame if their hitbox is enabled and hit something
        AttackScript.InitiateAttack();
    }
}
