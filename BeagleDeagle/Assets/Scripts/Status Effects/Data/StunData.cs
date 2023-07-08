using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStunEffect", menuName = "ScriptableObjects/StatusEffects/Stun")]
public class StunData : StatusEffectData
{
    [Range(0.1f, 15f)]
    public float stunDuration;

    // public override GameObject UpdateStatusEffects(GameObject objectWithStatusEffect, GameObject activator)
    // {
    //     StatusEffect<StunData> stunComponent = objectWithStatusEffect.GetComponent<StatusEffect<StunData>>();
    //     
    //     stunComponent.UpdateScriptableObject(this);
    //
    //     return objectWithStatusEffect;
    //
    //     // ALSO ADD STUNMODIFIERS
    // }
}
