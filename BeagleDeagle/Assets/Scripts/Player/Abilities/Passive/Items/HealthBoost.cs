using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoost : PassiveAbility
{
    [SerializeField] private HealthBoostData healthBoostData;
    protected override void ActivatePassive()
    {
        IHealthWithModifiers playerHealth = Player.GetComponent<IHealthWithModifiers>();
        
        Debug.Log("GIVE INCREASED HEALTH");
        
        // Add max health modifier to the player
        // Increases player's max health
        playerHealth?.AddMaxHealthModifier(healthBoostData.maxHealthModifier);
        
    }

    protected override void RemovePassive()
    {
        IHealthWithModifiers playerHealth = Player.GetComponent<IHealthWithModifiers>();
        
        // Add max health modifier to the player
        // Increases player's max health
        playerHealth?.RemoveMaxHealthModifier(healthBoostData.maxHealthModifier);
    }
}
