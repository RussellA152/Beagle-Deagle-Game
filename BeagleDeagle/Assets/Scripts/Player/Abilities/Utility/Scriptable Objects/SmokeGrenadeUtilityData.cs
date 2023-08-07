using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUtility", menuName = "ScriptableObjects/Ability/Utility/SmokeGrenade")]
public class SmokeGrenadeUtilityData : UtilityAbilityData
{
    [Header("Grenade Data")]
    [RestrictedPrefab(typeof(AreaGrenade))]
    public GameObject smokeGrenadePrefab;
    
    public UtilityExplosiveData utilityExplosiveData;

    // The slow effect of this smoke grenade
    public SlowData slowData;

}

