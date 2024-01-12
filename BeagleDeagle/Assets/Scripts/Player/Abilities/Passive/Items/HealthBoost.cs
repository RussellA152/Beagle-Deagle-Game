using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoost : PassiveAbility
{
    [SerializeField] private HealthBoostData healthBoostData;
    protected override void ActivatePassive()
    {
        ModifierManager playerHealth = Player.GetComponent<ModifierManager>();
        
        // Add max health modifier to the player
        // Increases player's max health
        playerHealth.AddModifier(healthBoostData.maxHealthModifier);

        DisplayPassiveOnBuffBar();
    }

    protected override void RemovePassive()
    {
        ModifierManager playerHealth = Player.GetComponent<ModifierManager>();
        
        // Add max health modifier to the player
        // Increases player's max health
        playerHealth.RemoveModifier(healthBoostData.maxHealthModifier);
        
        RemovePassiveFromBuffBar();
    }
}
