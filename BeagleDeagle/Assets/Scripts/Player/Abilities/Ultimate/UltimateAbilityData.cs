using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// A powerful ability that is unique to each playable character. Similar to utility abilities, an ultimate ability has a cooldown
/// before the player can use it again
/// </summary>

public abstract class UltimateAbilityData : AbilityData
{
    [Range(0f, 3f)]
    public float startTime;

    // public ActivationType activationType;
    //
    // public enum ActivationType
    // {
    //     Immediate,
    //     
    //     Duration
    // }

    //public abstract IEnumerator ActivateUltimate(GameObject player);
    

}
