using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoost : PassiveAbility<HealthBoostData>
{
    protected override void ActivatePassive()
    {
        IHealthWIthModifiers playerHealth = Player.GetComponent<IHealthWIthModifiers>();
        
        // Add max health modifier to the player
        // Increases player's max health
        playerHealth?.AddMaxHealthModifier(passiveData.maxHealthModifier);
        
    }

    protected override void RemovePassive()
    {
        IHealthWIthModifiers playerHealth = Player.GetComponent<IHealthWIthModifiers>();
        
        // Add max health modifier to the player
        // Increases player's max health
        playerHealth?.RemoveMaxHealthModifier(passiveData.maxHealthModifier);
    }
}
