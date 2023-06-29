using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "ScriptableObjects/Ability/Passive/HealthBoost")]
public class HealthBoost : PassiveAbilityData
{
    [Range(0f, 1f)]
    public float increaseAmount;

    public override void ActivatePassive(GameObject player)
    {
        IHealth playerHealth = player.GetComponent<IHealth>();
        
        // Add max health modifier to the player
        // Increases player's max health
        playerHealth?.AddMaxHealthModifier(new MaxHealthModifier(this.name, increaseAmount));
    }
}

