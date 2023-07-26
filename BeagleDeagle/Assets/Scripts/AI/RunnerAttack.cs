using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerAttack : AIAttack<RunnerEnemyData>
{
    public Collider2D hitBox;
    
    public override void InitiateAttack()
    {
        base.InitiateAttack();
        
    }
    
}
