using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewUtility", menuName = "ScriptableObjects/Ability/Utility/SmokeGrenade")]
public class SmokeGrenadeUtilityData : UtilityAbilityData
{
    [RestrictedPrefab(typeof(Grenade))]
    // The smoke grenade to spawn
    public GameObject smokeGrenadePrefab;

    // The data the smoke grenade will use
    public ExplosiveData smokeGrenadeData;
    
    // The data of the status effects that the smoke grenade will have ( * MUST BE COMPATIBLE WITH PREFAB * )
    public StatusEffectTypes statusEffects;

}

