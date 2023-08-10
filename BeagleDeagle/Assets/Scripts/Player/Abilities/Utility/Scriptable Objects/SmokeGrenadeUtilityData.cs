using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUtility", menuName = "ScriptableObjects/Ability/Utility/SmokeGrenade")]
public class SmokeGrenadeUtilityData : UtilityAbilityData
{
    [Header("Explosive Data")] 
    // Prefab of smoke grenade will come from ExplosiveType
    public ExplosiveTypeData explosiveType;

}

