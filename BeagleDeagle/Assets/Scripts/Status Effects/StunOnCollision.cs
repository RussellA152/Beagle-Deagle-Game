using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunOnCollision : StatusEffect<StunEffectData>
{
    public override void ApplyEffect(GameObject objectHit)
    {
        if (DoesThisAffectTarget(objectHit))
        {
            // Attempt to stun the enemy for a certain amount of seconds
            objectHit.GetComponent<ModifierManager>().AddModifier(StatusEffectData.stunEffect);
            
        }

    }
}
