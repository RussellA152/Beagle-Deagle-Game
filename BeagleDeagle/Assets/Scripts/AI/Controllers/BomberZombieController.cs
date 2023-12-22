using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberZombieController : AIController<BomberEnemyData>
{
    protected override void OnAttack()
    {
        // Don't allow bomber enemies to turn around during their attack
        MovementScript.SetCanFlip(false);
        base.OnAttack();
    }
}
