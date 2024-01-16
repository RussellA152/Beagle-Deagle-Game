using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCooldownPowerUp : PowerUp
{
    private ShowOnBuffBar _showOnBuffBar;
    private ModifierManager _modifierManager;
    
    [SerializeField] private UtilityCooldownModifier utilityCooldownModifier;
    [SerializeField] private UltimateCooldownModifier ultimateCooldownModifier;

    [SerializeField, Range(0.1f, 60f)] 
    private float cooldownBuffDuration;

    [SerializeField] private Sprite icon;

    protected override void Awake()
    {
        base.Awake();
        
        _showOnBuffBar = GetComponent<ShowOnBuffBar>();
        
        _showOnBuffBar.SetBuffIcon(icon);
        _showOnBuffBar.SetBuffModifier(utilityCooldownModifier);
    }

    protected override void OnPickUp(GameObject receiverGameObject)
    {
        _modifierManager = receiverGameObject.GetComponent<ModifierManager>();
        
        _showOnBuffBar.SetModifierManager(_modifierManager);
        
        
        _modifierManager.AddModifierOnlyForDuration(utilityCooldownModifier, cooldownBuffDuration);
        _modifierManager.AddModifierOnlyForDuration(ultimateCooldownModifier, cooldownBuffDuration);
        
        _showOnBuffBar.ShowBuffIconWithDuration(cooldownBuffDuration);
        
    }
    
    
}
