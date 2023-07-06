using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AbilityData : ScriptableObject
{
    public string abilityName;
    
    [Range(0, 1000f)]
    public float abilityDamage;

    [Header("Usage"), Range(0f,240f)]
    public float cooldown; // How long will it take for the player to be able to use this again?

    [Range(0f,240f)]
    public float duration; // How long smoke grenade lasts for & life time for mighty foot bullet
}
