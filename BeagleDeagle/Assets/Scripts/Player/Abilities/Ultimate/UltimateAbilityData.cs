using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A powerful ability that is unique to each playable character. Similar to utility abilities, an ultimate ability has a cooldown
/// before the player can use it again
/// </summary>
[CreateAssetMenu(fileName = "NewUltimate", menuName = "ScriptableObjects/Ability/Ultimate")]
public abstract class UltimateAbilityData : ScriptableObject
{
    [Header("Timers")]
    public float cooldown; // How long will it take for the player to be able to use this again?

    public float duration; // How long until this ultimate deactivates?

    //[HideInInspector]
    //public bool isActive;

    public abstract void ActivateUltimate(GameObject player);

    public abstract IEnumerator ActivationCooldown();

}
