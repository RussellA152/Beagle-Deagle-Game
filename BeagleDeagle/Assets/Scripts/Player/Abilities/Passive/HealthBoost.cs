using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoost : PassiveAbility<HealthBoostPassive>
{
    protected override void ActivatePassive()
    {
        IHealth playerHealth = player.GetComponent<IHealth>();
        
        // Add max health modifier to the player
        // Increases player's max health
        playerHealth?.AddMaxHealthModifier(passiveData.maxHealthModifier);
        
    }

    protected override void RemovePassive()
    {
        IHealth playerHealth = player.GetComponent<IHealth>();
        
        // Add max health modifier to the player
        // Increases player's max health
        playerHealth?.RemoveMaxHealthModifier(passiveData.maxHealthModifier);
    }
}
