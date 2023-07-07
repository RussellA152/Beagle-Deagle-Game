using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowOnCollision : StatusEffect<SlowData>
{
    public bool removeOnTriggerExit;

    public override void ApplyEffect(GameObject objectHit)
    {
        if (DoesThisAffectTarget(objectHit))
        {
            objectHit.GetComponent<IMovable>().AddMovementSpeedModifier(statusEffectData.movementSpeedEffect);
        
            objectHit.GetComponent<IDamager>().AddAttackSpeedModifier(statusEffectData.attackSpeedEffect);
        }
        
    }
    

    public void RemoveEffectFromTarget(GameObject objectHit)
    {
        if (DoesThisAffectTarget(objectHit) && removeOnTriggerExit)
        {
            objectHit.GetComponent<IMovable>().RemoveMovementSpeedModifier(statusEffectData.movementSpeedEffect);
        
            objectHit.GetComponent<IDamager>().RemoveAttackSpeedModifier(statusEffectData.attackSpeedEffect);
        }
        
    }

    
}
