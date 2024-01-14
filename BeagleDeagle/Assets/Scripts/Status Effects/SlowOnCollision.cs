using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowOnCollision : StatusEffect<SlowEffectData>
{
    
    public override void ApplyEffect(GameObject objectHit)
    {
        if (DoesThisAffectTarget(objectHit))
        {
            ModifierManager modifierManager = objectHit.GetComponent<ModifierManager>();
            
            if (StatusEffectData.lingerDurationType == SlowEffectData.LingerDurationType.OnEnter)
            {
                modifierManager.AddModifierOnlyForDuration(StatusEffectData.movementSpeedEffect, StatusEffectData.lingerDuration);
                modifierManager.AddModifierOnlyForDuration(StatusEffectData.attackSpeedEffect, StatusEffectData.lingerDuration);
            }
            else
            {
                modifierManager.AddModifier(StatusEffectData.movementSpeedEffect);
                modifierManager.AddModifier(StatusEffectData.attackSpeedEffect);
            }
            
        }
        
    }
    
    public void RemoveEffectFromTarget(GameObject objectHit)
    {
        if (DoesThisAffectTarget(objectHit))
        {
            ModifierManager modifierManager = objectHit.GetComponent<ModifierManager>();

            if (StatusEffectData.lingerDurationType == SlowEffectData.LingerDurationType.OnExit)
            {
                modifierManager.RemoveModifierAfterDelay(StatusEffectData.movementSpeedEffect, StatusEffectData.lingerDuration);
                modifierManager.RemoveModifierAfterDelay(StatusEffectData.attackSpeedEffect, StatusEffectData.lingerDuration);
            }
            else
            {
                modifierManager.RemoveModifier(StatusEffectData.movementSpeedEffect);
                modifierManager.RemoveModifier(StatusEffectData.attackSpeedEffect);
            }
            
        }
        
    }
    
    
}
