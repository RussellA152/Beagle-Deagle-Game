using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewNukeUltimate", menuName = "ScriptableObjects/Ability/Ultimate/Nuclear Bomb")]
public class NukeUltimateData : UltimateAbilityData
{
    [Header("Prefab to Spawn")]
    [RestrictedPrefab(typeof(Nuke))]
    public GameObject nukePrefab;

    [Header("Grenade Data")]
    public NukeData nukeData;

    
}
