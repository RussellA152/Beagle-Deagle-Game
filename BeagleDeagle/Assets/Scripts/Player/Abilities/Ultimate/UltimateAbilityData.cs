using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A powerful ability that is unique to each playable character. Similar to utility abilities, an ultimate ability has a cooldown
/// before the player can use it again
/// </summary>

public abstract class UltimateAbilityData : AbilityData
{

    [HideInInspector]
    public bool isActive;

    private void OnEnable()
    {
        isActive = false;
    }

    private void OnDisable()
    {
        isActive = false;
    }

    public abstract void ActivateUltimate(GameObject player);

    public abstract IEnumerator ActivationCooldown();

}
