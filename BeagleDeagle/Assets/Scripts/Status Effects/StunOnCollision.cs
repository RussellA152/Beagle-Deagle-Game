using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunOnCollision : StatusEffect<StunData>
{
    public override void ApplyEffect(GameObject objectHit)
    {
        if (DoesThisAffectTarget(objectHit))
        {
            // Stun the enemy for a certain amount of seconds
            objectHit.GetComponent<IStunnable>().GetStunned(StatusEffectData.stunDuration);
            
        }

    }
}
