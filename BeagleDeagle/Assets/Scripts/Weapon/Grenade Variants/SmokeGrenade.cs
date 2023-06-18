using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewExplosive", menuName = "ScriptableObjects/Explosive/Smoke Grenade")]
public class SmokeGrenade : GrenadeData
{
    [SerializeField]
    private SmokeBombUtility utilityAbilityData;

    public override float GetDuration()
    {
        return utilityAbilityData.duration;
    }
}
