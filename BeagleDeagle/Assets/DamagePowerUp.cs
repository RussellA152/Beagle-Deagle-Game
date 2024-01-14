using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePowerUp : PowerUp
{
    [SerializeField] private DamageModifier damageModifier;

    [Range(0.1f, 60f)]
    [SerializeField] private float damageBuffDuration;
    
    protected override void OnPickUp(GameObject receiverGameObject)
    {
        ModifierManager modifierManager = receiverGameObject.GetComponent<ModifierManager>();
        
        modifierManager.AddModifierOnlyForDuration(damageModifier, damageBuffDuration);
    }
}
