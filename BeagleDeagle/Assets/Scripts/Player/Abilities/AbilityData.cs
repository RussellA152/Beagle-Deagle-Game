using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityData : ScriptableObject
{
    public float abilityDamage;

    [Header("Usage")]
    public float cooldown; // How long will it take for the player to be able to use this again?

    public float duration; // How long smoke grenade lasts for & life time for mighty foot bullet
}
