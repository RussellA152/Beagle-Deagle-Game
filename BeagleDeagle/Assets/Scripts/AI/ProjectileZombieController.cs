using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileZombieController : AIController<ProjectileEnemyData>
{
    protected override void OnAttack()
    {
        base.OnAttack();
        
        MovementScript.SetCanFlip(true);
    }
}
