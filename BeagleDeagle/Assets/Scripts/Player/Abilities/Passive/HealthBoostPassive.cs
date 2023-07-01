using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "ScriptableObjects/Ability/Passive/HealthBoost")]
public class HealthBoostPassive : PassiveAbilityData
{
    [Range(0f, 1f)]
    public float increaseAmount;

    private MaxHealthModifier _maxHealthModifier;
    private void OnEnable()
    {
        _maxHealthModifier = new MaxHealthModifier(this.name, increaseAmount);
    }

    public override IEnumerator ActivatePassive(GameObject player)
    {
        IHealth playerHealth = player.GetComponent<IHealth>();
        
        // Add max health modifier to the player
        // Increases player's max health
        playerHealth?.AddMaxHealthModifier(_maxHealthModifier);
        
        yield return null;
    }

    public override void RemovePassive(GameObject player)
    {
        IHealth playerHealth = player.GetComponent<IHealth>();
        
        // Add max health modifier to the player
        // Increases player's max health
        playerHealth?.RemoveMaxHealthModifier(_maxHealthModifier);
    }
}

