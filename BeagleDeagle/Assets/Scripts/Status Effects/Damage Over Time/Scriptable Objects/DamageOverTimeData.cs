using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDamageOverTime", menuName = "ScriptableObjects/StatusEffects/DamageOverTime")]
public class DamageOverTimeData : StatusEffectData
{
    public DamageOverTime damageOverTime;
    
    // public override GameObject UpdateStatusEffects(GameObject objectWithStatusEffect, GameObject activator)
    // {
    //     StatusEffect<DamageOverTimeData> damageOverTimeComponent = objectWithStatusEffect.GetComponent<StatusEffect<DamageOverTimeData>>();
    //     
    //     damageOverTimeComponent.UpdateScriptableObject(this);
    //
    //     return objectWithStatusEffect;
    //
    // }
}
