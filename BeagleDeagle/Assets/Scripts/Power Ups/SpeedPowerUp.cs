using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpeedPowerUp : PowerUp
{
    private ShowOnBuffBar _showOnBuffBar;
    private ModifierManager _modifierManager;
    
    [SerializeField] private MovementSpeedBoostData movementSpeedBoostData;
    [SerializeField] private AttackSpeedBoostData attackSpeedBoostData;
    
    [Range(0.1f, 60f)]
    [SerializeField] private float speedBuffDuration;

    [SerializeField] private Sprite icon;

    protected override void Awake()
    {
        base.Awake();
        
        _showOnBuffBar = GetComponent<ShowOnBuffBar>();
        
        _showOnBuffBar.SetBuffIcon(icon);
        _showOnBuffBar.SetBuffModifier(movementSpeedBoostData.movementSpeedModifier);
        
        _showOnBuffBar.onRemovedFromBuffBar += Deactivate;
    }

    protected override void OnPickUp(GameObject receiverGameObject)
    {
        _modifierManager = receiverGameObject.GetComponent<ModifierManager>();

        _showOnBuffBar.SetModifierManager(_modifierManager);
        
        // If player already has power up, refresh the timer
        if (_modifierManager.DoesEntityContainModifier(movementSpeedBoostData.movementSpeedModifier) ||
            _modifierManager.DoesEntityContainModifier(attackSpeedBoostData.attackSpeedModifier))
        {
            _modifierManager.RefreshRemoveModifierTimer(movementSpeedBoostData.movementSpeedModifier, speedBuffDuration);
            _modifierManager.RefreshRemoveModifierTimer(attackSpeedBoostData.attackSpeedModifier, speedBuffDuration);
        }
        // Otherwise, give them buff for the first time
        else
        {
            _modifierManager.AddModifierOnlyForDuration(movementSpeedBoostData.movementSpeedModifier, speedBuffDuration);
            _modifierManager.AddModifierOnlyForDuration(attackSpeedBoostData.attackSpeedModifier, speedBuffDuration);
            

        }
        
        _showOnBuffBar.ShowBuffIconWithDuration(speedBuffDuration);
        
    }
    
    protected override void Deactivate()
    {
        base.Deactivate();
        
        _showOnBuffBar.onRemovedFromBuffBar -= Deactivate;
    }

}
