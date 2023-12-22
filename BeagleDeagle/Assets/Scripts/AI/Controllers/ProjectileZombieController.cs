using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileZombieController : AIController<ProjectileEnemyData>
{
    protected override void OnAttack()
    {
        // Unlike runner and bomber, allow projectile enemies to turn around during their attacks
        MovementScript.SetCanFlip(true);
        
        base.OnAttack();
        
    }
}
