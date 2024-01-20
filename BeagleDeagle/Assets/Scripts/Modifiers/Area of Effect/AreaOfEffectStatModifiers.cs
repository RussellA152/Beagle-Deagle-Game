using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AreaOfEffectRadiusModifier : Modifier
{
    [Range(-1f, 1f)]
    public float bonusRadius;

    ///-///////////////////////////////////////////////////////////
    /// An increase or decrease applied to an player's aoe radius values.
    /// 
    public AreaOfEffectRadiusModifier(string name, float radius)
    {
        modifierName = name;
        bonusRadius = radius;
    }
}

[System.Serializable]
public class DamageOverTimeDamageModifier : Modifier
{
    [Range(-3f, 3f)]
    public float bonusDamage;

    ///-///////////////////////////////////////////////////////////
    /// An increase or decrease applied to an player's aoe damage values
    /// 
    public DamageOverTimeDamageModifier(string name, float damage)
    {
        modifierName = name;
        bonusDamage = damage;
    }
}
