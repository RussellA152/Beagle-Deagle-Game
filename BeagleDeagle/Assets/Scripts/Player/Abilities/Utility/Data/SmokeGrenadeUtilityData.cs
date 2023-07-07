using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewUtility", menuName = "ScriptableObjects/Ability/Utility/SmokeGrenade")]
public class SmokeGrenadeUtilityData : UtilityAbilityData
{
    [Header("Grenade Data")]
    [RestrictedPrefab(typeof(AreaGrenade))]
    public GameObject smokeGrenadePrefab;
    
    public UtilityGrenadeData utilityGrenadeData;

    public SlowData slowData;

}

