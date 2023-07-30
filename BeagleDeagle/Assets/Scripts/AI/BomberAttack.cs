using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberAttack : AIAttack<BomberEnemyData>
{
    public override void InitiateAttack()
    {
        
        Debug.Log("EXPLODE!");
        
    }
}
