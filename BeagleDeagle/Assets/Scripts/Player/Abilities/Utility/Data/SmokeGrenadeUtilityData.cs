using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewUtility", menuName = "ScriptableObjects/Ability/Utility/SmokeBomb")]
public class SmokeGrenadeUtilityData : UtilityAbilityData
{
    [Header("Grenade Data")]
    [RestrictedPrefab(typeof(SmokeGrenade))]
    public GameObject smokeGrenadePrefab;
    
    public SmokeGrenadeData smokeGrenadeData;
    
}

