using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpeedPowerUp : PowerUp
{
    [SerializeField] private MovementSpeedBoostData movementSpeedBoostData;
    [SerializeField] private AttackSpeedBoostData attackSpeedBoostData;
    
    [Range(0.1f, 60f)]
    [SerializeField] private float speedBuffDuration;
    protected override void OnPickUp(GameObject receiverGameObject)
    {
        ModifierManager modifierManager = receiverGameObject.GetComponent<ModifierManager>();
        
        modifierManager.AddModifierOnlyForDuration(movementSpeedBoostData.movementSpeedModifier, speedBuffDuration);
        modifierManager.AddModifierOnlyForDuration(attackSpeedBoostData.attackSpeedModifier, speedBuffDuration);
        
    }
}
