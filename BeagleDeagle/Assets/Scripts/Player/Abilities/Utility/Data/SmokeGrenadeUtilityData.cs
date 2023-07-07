using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewUtility", menuName = "ScriptableObjects/Ability/Utility/SmokeGrenade")]
public class SmokeGrenadeUtilityData : UtilityAbilityData
{
    [Header("Grenade Data")]
    [RestrictedPrefab(typeof(Grenade))]
    public GameObject smokeGrenadePrefab;
    
    public SmokeGrenadeData smokeGrenadeData;

    public SlowData slowData;

}

