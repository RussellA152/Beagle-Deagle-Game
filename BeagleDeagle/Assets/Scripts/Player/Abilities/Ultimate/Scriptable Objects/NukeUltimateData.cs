using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewNukeUltimate", menuName = "ScriptableObjects/Ability/Ultimate/Nuclear Bomb")]
public class NukeUltimateData : UltimateAbilityData
{

    [Header("Explosive Data")] 
    // Prefab of nuke bomb will come from ExplosiveType
    public ExplosiveTypeData explosiveType;

}
