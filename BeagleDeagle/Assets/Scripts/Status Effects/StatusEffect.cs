using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StatusEffect<T> : MonoBehaviour, IStatusEffect where T: StatusEffectData
{
    [SerializeField]
    protected T statusEffectData;

    public abstract void ApplyEffect(GameObject objectHit);

    protected bool DoesThisAffectTarget(GameObject objectHit)
    {
        if ((statusEffectData.whatStatusEffectHits.value & (1 << objectHit.layer)) > 0)
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
            statusEffectData = scriptableObject as T;
        }
        else
        {
            Debug.LogError("ERROR WHEN UPDATING SCRIPTABLE OBJECT! " + scriptableObject + " IS NOT A " + typeof(T));
        }
    }
}
