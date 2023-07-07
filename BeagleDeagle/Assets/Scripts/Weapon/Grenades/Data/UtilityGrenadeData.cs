using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewExplosive", menuName = "ScriptableObjects/Explosive/SmokeGrenade")]
public class UtilityGrenadeData : GrenadeData
{
    [SerializeField]
    private UtilityAbilityData utilityAbilityData;
    
    public override float GetDamage()
    {
        return utilityAbilityData.abilityDamage;
    }

    public override float GetDuration()
    {
        return utilityAbilityData.duration;
    }
}
