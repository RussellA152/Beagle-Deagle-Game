using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePowerUp : PowerUp
{
    private ShowOnBuffBar _showOnBuffBar;
    private ModifierManager _modifierManager;
    
    [SerializeField] private DamageModifierData damageModifierData;

    [Range(0.1f, 60f)]
    [SerializeField] private float damageBuffDuration;
    
    
    [SerializeField] private Sprite icon;
    
    protected override void Awake()
    {
        base.Awake();

        _showOnBuffBar = GetComponent<ShowOnBuffBar>();
        
        _showOnBuffBar.SetBuffIcon(icon);
        _showOnBuffBar.SetBuffModifier(damageModifierData.damageModifier);
        
        _showOnBuffBar.onRemovedFromBuffBar += Deactivate;
    }
    
    protected override void OnPickUp(GameObject receiverGameObject)
    {
        _modifierManager = receiverGameObject.GetComponent<ModifierManager>();

        _showOnBuffBar.SetModifierManager(_modifierManager);

        // If player already has power up, refresh the timer
        if (_modifierManager.DoesEntityContainModifier(damageModifierData.damageModifier))
        {
            _modifierManager.RefreshRemoveModifierTimer(damageModifierData.damageModifier, damageBuffDuration);
        }
        // Otherwise, give them buff for the first time
        else
        {
            _modifierManager.AddModifierOnlyForDuration(damageModifierData.damageModifier, damageBuffDuration);


        }
        _showOnBuffBar.ShowBuffIconWithDuration(damageBuffDuration);
    }
    
    protected override void Deactivate()
    {
        base.Deactivate();
        
        _showOnBuffBar.onRemovedFromBuffBar -= Deactivate;
    }
    
}
