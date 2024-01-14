using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCooldownPowerUp : PowerUp
{
    [SerializeField] private UtilityCooldownModifier utilityCooldownModifier;
    [SerializeField] private UltimateCooldownModifier ultimateCooldownModifier;

    [SerializeField, Range(0.1f, 60f)] 
    private float cooldownBuffDuration;
    protected override void OnPickUp(GameObject receiverGameObject)
    {
        ModifierManager modifierManager = receiverGameObject.GetComponent<ModifierManager>();
        
        modifierManager.AddModifierOnlyForDuration(utilityCooldownModifier, cooldownBuffDuration);
        modifierManager.AddModifierOnlyForDuration(ultimateCooldownModifier, cooldownBuffDuration);
    }
}
