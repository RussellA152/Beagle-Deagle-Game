using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowOnCollision : StatusEffect<SlowEffectData>
{
    public bool removeOnTriggerExit;

    public override void ApplyEffect(GameObject objectHit)
    {
        if (DoesThisAffectTarget(objectHit))
        {
            objectHit.GetComponent<IMovable>().AddMovementSpeedModifier(StatusEffectData.movementSpeedEffect);
        
            objectHit.GetComponent<IDamager>().AddAttackSpeedModifier(StatusEffectData.attackSpeedEffect);
        }
        
    }
    
    public void RemoveEffectFromTarget(GameObject objectHit)
    {
        if (DoesThisAffectTarget(objectHit) && removeOnTriggerExit)
        {
            objectHit.GetComponent<IMovable>().RemoveMovementSpeedModifier(StatusEffectData.movementSpeedEffect);
        
            objectHit.GetComponent<IDamager>().RemoveAttackSpeedModifier(StatusEffectData.attackSpeedEffect);
            
            //StartCoroutine(WaitToRemoveEffect(objectHit));
        }
        
    }

    // private IEnumerator WaitToRemoveEffect(GameObject objectHit)
    // {
    //     yield return new WaitForSeconds(StatusEffectData.lingerDuration);
    //     
    //     objectHit.GetComponent<IMovable>().RemoveMovementSpeedModifier(StatusEffectData.movementSpeedEffect);
    //     
    //     objectHit.GetComponent<IDamager>().RemoveAttackSpeedModifier(StatusEffectData.attackSpeedEffect);
    // }
    
}
