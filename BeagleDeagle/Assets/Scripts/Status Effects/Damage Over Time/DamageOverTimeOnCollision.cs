using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeOnCollision : StatusEffect<DamageOverTimeData>
{
    public override void ApplyEffect(GameObject objectHit)
    {
        if (DoesThisAffectTarget(objectHit))
        {
            IDamageOverTimeHandler damageOverTimeScript = objectHit.GetComponent<IDamageOverTimeHandler>();

            damageOverTimeScript?.AddDamageOverTime(statusEffectData.damageOverTime);
        }
        
    }
}
