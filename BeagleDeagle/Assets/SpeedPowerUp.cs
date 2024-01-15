using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpeedPowerUp : PowerUp
{
    private ShowOnBuffBar _showOnBuffBar;
    private ModifierManager _modifierManager;
    
    [SerializeField] private MovementSpeedModifier movementSpeedModifier;
    [SerializeField] private AttackSpeedModifier attackSpeedModifier;
    
    [Range(0.1f, 60f)]
    [SerializeField] private float speedBuffDuration;

    [SerializeField] private Sprite icon;

    protected override void Awake()
    {
        base.Awake();
        
        _showOnBuffBar = GetComponent<ShowOnBuffBar>();
        
        _showOnBuffBar.SetBuffIcon(icon);
        _showOnBuffBar.SetBuffModifier(movementSpeedModifier);
    }

    protected override void OnPickUp(GameObject receiverGameObject)
    {
        _modifierManager = receiverGameObject.GetComponent<ModifierManager>();
        
        _showOnBuffBar.SetModifierManager(_modifierManager);
        
        _modifierManager.AddModifierOnlyForDuration(movementSpeedModifier, speedBuffDuration);
        _modifierManager.AddModifierOnlyForDuration(attackSpeedModifier, speedBuffDuration);
        
        
        _showOnBuffBar.ShowBuffIconWithDuration(speedBuffDuration);
        
        
    }

}
