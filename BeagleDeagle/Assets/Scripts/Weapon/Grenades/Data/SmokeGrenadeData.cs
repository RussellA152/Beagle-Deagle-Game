using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewExplosive", menuName = "ScriptableObjects/Explosive/Smoke Grenade")]
public class SmokeGrenadeData : GrenadeData
{
    [SerializeField]
    private SmokeGrenadeUtilityData utilityAbilityData;

    //public SmokeAreaOfEffectData smokeAOEData;

    public override float GetDamage()
    {
        return utilityAbilityData.abilityDamage;
    }

    public override float GetDuration()
    {
        return utilityAbilityData.duration;
    }
}
