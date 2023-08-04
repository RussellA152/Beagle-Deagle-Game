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
    
    [FormerlySerializedAs("utilityGrenadeData")] public UtilityExplosiveData utilityExplosiveData;

    public SlowData slowData;

}

