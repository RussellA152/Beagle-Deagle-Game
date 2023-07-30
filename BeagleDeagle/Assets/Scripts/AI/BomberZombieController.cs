using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberZombieController : AIController<BomberEnemyData>
{
    protected override void OnAttack()
    {
        MovementScript.SetCanFlip(false);
        base.OnAttack();
    }
}
