using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StatusEffect<T> : MonoBehaviour, IStatusEffect where T: StatusEffectData
{

    private StatusEffectTypes _statusEffectTypes;
    protected T StatusEffectData;

    ///-///////////////////////////////////////////////////////////
    /// Apply an effect to the object that was hit by the status effect.
    /// Example effects are slow, stun, and damage over time.
    /// 
    public abstract void ApplyEffect(GameObject objectHit);

    ///-///////////////////////////////////////////////////////////
    /// Check if the target is able to be affected by this specific status effect.
    /// For example, the radiation from the nuke does not apply to the player, so they do not get damaged.
    /// 
    protected bool DoesThisAffectTarget(GameObject objectHit)
    {
        if ((StatusEffectData.whatStatusEffectHits.value & (1 << objectHit.layer)) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void UpdateScriptableObject(StatusEffectData scriptableObject)
    {
        if (scriptableObject is T)
        {
            StatusEffectData = scriptableObject as T;
        }
        else
        {
            Debug.LogError("ERROR WHEN UPDATING SCRIPTABLE OBJECT! " + scriptableObject + " IS NOT A " + typeof(T));
        }
    }

    public void UpdateWeaponType(StatusEffectTypes statusEffectTypesScriptableObject)
    {
        _statusEffectTypes = statusEffectTypesScriptableObject;
        
        // Retrieve status effect data from explosive type
        // Examples: StunData, DamageOverTimeData, SlowData
        StatusEffectData = _statusEffectTypes.GetStatusEffectData<T>();
    }
}
