using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewNukeUltimate", menuName = "ScriptableObjects/Ability/Ultimate/Nuclear Bomb")]
public class NukeUltimateData : UltimateAbilityData
{
    [RestrictedPrefab(typeof(Nuke))]
    // The nuke prefab to spawn
    public GameObject nukePrefab;

    // The data the nuke will use
    public ExplosiveData nukeData;
    
    // The data of the status effects that the nuke will have ( * MUST BE COMPATIBLE WITH PREFAB * )
    public StatusEffectTypes statusEffects;

}
